using Robust.Shared.Serialization;
// ReSharper disable InconsistentNaming

namespace Content.Shared.Atmos
{
    /// <summary>
    ///     Class to store atmos constants.
    /// </summary>
    public static class Atmospherics
    {
        #region ATMOS
        /// <summary>
        ///     The universal gas constant, in kPa*L/(K*mol)
        /// </summary>
        public const float R = 8.314462618f;

        /// <summary>
        ///     1 ATM in kPA.
        /// </summary>
        public const float OneAtmosphere = 101.325f;

        /// <summary>
        ///     Maximum external pressure (in kPA) a gas miner will, by default, output to.
        ///     This is used to initialize roundstart atmos rooms.
        /// </summary>
        public const float GasMinerDefaultMaxExternalPressure = 6500f;

        /// <summary>
        ///     -270.3ºC in K. CMB stands for Cosmic Microwave Background.
        /// </summary>
        public const float TCMB = 2.7f;

        /// <summary>
        ///     0ºC in K
        /// </summary>
        public const float T0C = 273.15f;

        /// <summary>
        ///     20ºC in K
        /// </summary>
        public const float T20C = 293.15f;

        /// <summary>
        ///     -38.15ºC in K.
        ///     This is used to initialize roundstart freezer rooms.
        /// </summary>
        public const float FreezerTemp = 235f;

        /// <summary>
        ///     Do not allow any gas mixture temperatures to exceed this number. It is occasionally possible
        ///     to have very small heat capacity (e.g. room that was just unspaced) and for large amounts of
        ///     energy to be transferred to it, even for a brief moment. However, this messes up subsequent
        ///     calculations and so cap it here. The physical interpretation is that at this temperature, any
        ///     gas that you would have transforms into plasma.
        /// </summary>
        public const float Tmax = 262144; // 1/64 of max safe integer, any values above will result in a ~0.03K epsilon

        /// <summary>
        ///     Liters in a cell.
        /// </summary>
        public const float CellVolume = 2500f;

        // Liters in a normal breath
        public const float BreathVolume = 0.5f;

        // Amount of air to take from a tile
        public const float BreathPercentage = BreathVolume / CellVolume;

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at 101.325 kPa and 20ºC
        /// </summary>
        public const float MolesCellStandard = (OneAtmosphere * CellVolume / (T20C * R));

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at 101.325 kPa and -38.15ºC.
        ///     This is used in fix atmos freezer markers to ensure the air is at the correct atmospheric pressure while still being cold.
        /// </summary>
        public const float MolesCellFreezer = (OneAtmosphere * CellVolume / (FreezerTemp * R));

        /// <summary>
        ///     Moles in a 2.5 m^3 cell at GasMinerDefaultMaxExternalPressure kPa and 20ºC
        /// </summary>
        public const float MolesCellGasMiner = (GasMinerDefaultMaxExternalPressure * CellVolume / (T20C * R));

        /// <summary>
        ///     Compared against for superconduction.
        /// </summary>
        public const float MCellWithRatio = (MolesCellStandard * 0.005f);

        public const float OxygenStandard = 0.21f;
        public const float NitrogenStandard = 0.79f;

        public const float OxygenMolesStandard = MolesCellStandard * OxygenStandard;
        public const float NitrogenMolesStandard = MolesCellStandard * NitrogenStandard;

        public const float OxygenMolesFreezer = MolesCellFreezer * OxygenStandard;
        public const float NitrogenMolesFreezer = MolesCellFreezer * NitrogenStandard;

        #endregion

        /// <summary>
        ///     Visible moles multiplied by this factor to get moles at which gas is at max visibility.
        /// </summary>
        public const float FactorGasVisibleMax = 20f;

        /// <summary>
        ///     Minimum number of moles a gas can have.
        /// </summary>
        public const float GasMinMoles = 0.00000005f;

        public const float OpenHeatTransferCoefficient = 0.4f;

        /// <summary>
        ///     Hack to make vacuums cold, sacrificing realism for gameplay.
        /// </summary>
        public const float HeatCapacityVacuum = 7000f;

        /// <summary>
        ///     Ratio of air that must move to/from a tile to reset group processing
        /// </summary>
        public const float MinimumAirRatioToSuspend = 0.1f;

        /// <summary>
        ///     Minimum ratio of air that must move to/from a tile
        /// </summary>
        public const float MinimumAirRatioToMove = 0.001f;

        /// <summary>
        ///     Minimum amount of air that has to move before a group processing can be suspended
        /// </summary>
        public const float MinimumAirToSuspend = (MolesCellStandard * MinimumAirRatioToSuspend);

        public const float MinimumTemperatureToMove = (T20C + 100f);

        public const float MinimumMolesDeltaToMove = (MolesCellStandard * MinimumAirRatioToMove);

        /// <summary>
        ///     Minimum temperature difference before group processing is suspended
        /// </summary>
        public const float MinimumTemperatureDeltaToSuspend = 4.0f;

        /// <summary>
        ///     Minimum temperature difference before the gas temperatures are just set to be equal.
        /// </summary>
        public const float MinimumTemperatureDeltaToConsider = 0.01f;

        /// <summary>
        ///     Minimum temperature for starting superconduction.
        /// </summary>
        public const float MinimumTemperatureStartSuperConduction = (T20C + 400f);
        public const float MinimumTemperatureForSuperconduction = (T20C + 80f);

        /// <summary>
        ///     Minimum heat capacity.
        /// </summary>
        public const float MinimumHeatCapacity = 0.0003f;

        /// <summary>
        ///     For the purposes of making space "colder"
        /// </summary>
        public const float SpaceHeatCapacity = 7000f;

        /// <summary>
        ///     Dictionary of chemical abbreviations for <see cref="Gas"/>
        /// </summary>
        public static Dictionary<Gas, string> GasAbbreviations = new Dictionary<Gas, string>()
        {
            [Gas.Ammonia] = Loc.GetString("gas-ammonia-abbreviation"),
            [Gas.BZ] = Loc.GetString("gas-bz-abbreviation"), // Funky/Goob - Ported gas
            [Gas.CarbonDioxide] = Loc.GetString("gas-carbon-dioxide-abbreviation"),
            [Gas.Frezon] = Loc.GetString("gas-frezon-abbreviation"),
            [Gas.Healium] = Loc.GetString("gas-healium-abbreviation"), // Funky/Goob - Ported gas
            [Gas.Nitrium] = Loc.GetString("gas-nitrium-abbreviation"), // Funky/Goob - Ported gas
            [Gas.Nitrogen] = Loc.GetString("gas-nitrogen-abbreviation"),
            [Gas.NitrousOxide] = Loc.GetString("gas-nitrous-oxide-abbreviation"),
            [Gas.Oxygen] = Loc.GetString("gas-oxygen-abbreviation"),
            [Gas.Plasma] = Loc.GetString("gas-plasma-abbreviation"),
			[Gas.Pluoxium] = Loc.GetString("gas-pluoxium-abbreviation"), // Funky/Goob - Ported gas
            [Gas.Tritium] = Loc.GetString("gas-tritium-abbreviation"),
            [Gas.WaterVapor] = Loc.GetString("gas-water-vapor-abbreviation"),
        };


        /// <summary>
        ///     Funkystation/Goob - Dictionary of names for <see cref="Gas"/>
        /// </summary>
        public static Dictionary<Gas, string> GasNames = new Dictionary<Gas, string>()
        {
            [Gas.Ammonia] = Loc.GetString("gases-ammonia"),
            [Gas.BZ] = Loc.GetString("gases-bz"), // Funky/Goob - Ported gas
            [Gas.CarbonDioxide] = Loc.GetString("gases-co2"),
            [Gas.Frezon] = Loc.GetString("gases-frezon"),
            [Gas.Healium] = Loc.GetString("gases-healium"), // Funky/Goob - Ported gas
            [Gas.Nitrium] = Loc.GetString("gases-nitrium"), // Funky/Goob - Ported gas
            [Gas.Nitrogen] = Loc.GetString("gases-nitrogen"),
            [Gas.NitrousOxide] = Loc.GetString("gases-n2o"),
            [Gas.Oxygen] = Loc.GetString("gases-oxygen"),
            [Gas.Plasma] = Loc.GetString("gases-plasma"),
            [Gas.Pluoxium] = Loc.GetString("gases-pluoxium"), // Funky/Goob - Ported gas
            [Gas.Tritium] = Loc.GetString("gases-tritium"),
            [Gas.WaterVapor] = Loc.GetString("gases-water-vapor"),
        };

        #region Excited Groups

        /// <summary>
        ///     Number of full atmos updates ticks before an excited group breaks down (averages gas contents across turfs)
        /// </summary>
        public const int ExcitedGroupBreakdownCycles = 4;

        /// <summary>
        ///     Number of full atmos updates before an excited group dismantles and removes its turfs from active
        /// </summary>
        public const int ExcitedGroupsDismantleCycles = 16;

        #endregion

        /// <summary>
        ///     Hard limit for zone-based tile equalization.
        /// </summary>
        public const int MonstermosHardTileLimit = 2000;

        /// <summary>
        ///     Limit for zone-based tile equalization.
        /// </summary>
        public const int MonstermosTileLimit = 200;

        /// <summary>
        ///     Total number of gases. Increase this if you want to add more!
        /// </summary>
        public const int TotalNumberOfGases = 185; //Funky/Goob: 9 >> 13

        /// <summary>
        ///     This is the actual length of the gases arrays in mixtures.
        ///     Set to the closest multiple of 4 relative to <see cref="TotalNumberOfGases"/> for SIMD reasons.
        /// </summary>
        public const int AdjustedNumberOfGases = ((TotalNumberOfGases + 3) / 4) * 4;

        /// <summary>
        ///     Amount of heat released per mole of burnt hydrogen or tritium (hydrogen isotope)
        /// </summary>
        public const float FireHydrogenEnergyReleased = 284e3f; // hydrogen is 284 kJ/mol
        public const float FireMinimumTemperatureToExist = T0C + 100f;
        public const float FireMinimumTemperatureToSpread = T0C + 150f;
        public const float FireSpreadRadiosityScale = 0.85f;
        public const float FirePlasmaEnergyReleased = 160e3f; // methane is 16 kJ/mol, plus plasma's spark of magic
        public const float FireGrowthRate = 40000f;

        public const float SuperSaturationThreshold = 30f; // Frontier: 96f<30
        public const float SuperSaturationEnds = 10f; // Frontier: SuperSaturationThreshold / 3 < 10

        public const float OxygenBurnRateBase = 1.4f;
        public const float PlasmaMinimumBurnTemperature = (100f+T0C);
        public const float PlasmaUpperTemperature = 700; // Frontier: (1370f+T0C)<700
        public const float PlasmaOxygenFullburn = 10f;
        public const float PlasmaBurnRateDelta = 9f;

        /// <summary>
        ///     This is calculated to help prevent singlecap bombs (Overpowered tritium/oxygen single tank bombs)
        /// </summary>
        public const float MinimumTritiumOxyburnEnergy = 143000f;

        public const float TritiumBurnOxyFactor = 100f;
        public const float TritiumBurnTritFactor = 10f;

        public const float FrezonCoolLowerTemperature = 23.15f;

        /// <summary>
        ///     Frezon cools better at higher temperatures.
        /// </summary>
        public const float FrezonCoolMidTemperature = 373.15f;

        public const float FrezonCoolMaximumEnergyModifier = 10f;

        /// <summary>
        ///     Remove X mol of nitrogen for each mol of frezon.
        /// </summary>
        public const float FrezonNitrogenCoolRatio = 5;
        public const float FrezonCoolEnergyReleased = -600e3f;
        public const float FrezonCoolRateModifier = 20f;

        public const float FrezonProductionMaxEfficiencyTemperature = 73.15f;

        /// <summary>
        ///     1 mol of N2 is required per X mol of tritium and oxygen.
        /// </summary>
        public const float FrezonProductionNitrogenRatio = 10f;

        /// <summary>
        ///     1 mol of Tritium is required per X mol of oxygen.
        /// </summary>
        public const float FrezonProductionTritRatio = 8.0f;

        /// <summary>
        ///     1 / X of the tritium is converted into Frezon each tick
        /// </summary>
        public const float FrezonProductionConversionRate = 50f;

        /// <summary>
        ///     The maximum portion of the N2O that can decompose each reaction tick. (50%)
        /// </summary>
        public const float N2ODecompositionRate = 2f;

        /// <summary>
        ///     Divisor for Ammonia Oxygen reaction so that it doesn't happen instantaneously.
        /// </summary>
        public const float AmmoniaOxygenReactionRate = 10f;
		
		///Funky/Goob start
		
        /// <summary>
        ///     The amount of energy 1 mole of BZ forming from N2O and plasma releases.
        /// </summary>
        public const float BZFormationEnergy = 80e3f;

        /// <summary>
        ///     The amount of energy 1 mol of Healium forming from BZ and frezon releases.
        /// </summary>
        public const float HealiumProductionEnergy = 9e3f;

        /// <summary>
        ///     The amount of energy 1 mol of Nitrium forming from Tritium, Nitrogen and BZ releases.
        /// </summary>
        public const float NitriumProductionEnergy = -100e3f;

        /// <summary>
        ///     The amount of energy 1 mol of Nitrium decomposing into nitrogen and water vapor releases.
        /// </summary>
        public const float NitriumDecompositionEnergy = 30e3f;
		
		/// <summary>
        ///     The amount of energy 1 mol of Pluoxium forming releases.
        /// </summary>
        public const float PluoxiumProductionEnergy = 250;
		
		///Funky/Goob end
		
        /// <summary>
        ///     Determines at what pressure the ultra-high pressure red icon is displayed.
        /// </summary>
        public const float HazardHighPressure = 550f;

        /// <summary>
        ///     Determines when the orange pressure icon is displayed.
        /// </summary>
        public const float WarningHighPressure = 0.7f * HazardHighPressure;

        /// <summary>
        ///     Determines when the gray low pressure icon is displayed.
        /// </summary>
        public const float WarningLowPressure = 2.5f * HazardLowPressure;

        /// <summary>
        ///     Determines when the black ultra-low pressure icon is displayed.
        /// </summary>
        public const float HazardLowPressure = 20f;

        /// <summary>
        ///    The amount of pressure damage someone takes is equal to ((pressure / HAZARD_HIGH_PRESSURE) - 1)*PRESSURE_DAMAGE_COEFFICIENT,
        ///     with the maximum of MaxHighPressureDamage.
        /// </summary>
        public const float PressureDamageCoefficient = 4;

        /// <summary>
        ///     Maximum amount of damage that can be endured with high pressure.
        /// </summary>
        public const int MaxHighPressureDamage = 4;

        /// <summary>
        ///     The amount of damage someone takes when in a low pressure area
        ///     (The pressure threshold is so low that it doesn't make sense to do any calculations,
        ///     so it just applies this flat value).
        /// </summary>
        // Original value is 4, buff back when we have proper ways for players to deal with breaches.
        public const int LowPressureDamage = 4; // Frontier: 1<4

        public const float WindowHeatTransferCoefficient = 0.1f;

        /// <summary>
        ///     Directions that atmos currently supports. Modify in case of multi-z.
        ///     See <see cref="AtmosDirection"/> on the server.
        /// </summary>
        public const int Directions = 4;

        /// <summary>
        ///     The normal body temperature in degrees Celsius.
        /// </summary>
        public const float NormalBodyTemperature = 37f;

        /// <summary>
        ///     I hereby decree. This is Arbitrary Suck my Dick
        /// </summary>
        public const float BreathMolesToReagentMultiplier = 1144;

        #region Pipes

        /// <summary>
        ///     The default pressure at which pumps and powered equipment max out at, in kPa.
        /// </summary>
        public const float MaxOutputPressure = 4500;

        /// <summary>
        ///     The default maximum speed powered equipment can work at, in L/s.
        /// </summary>
        public const float MaxTransferRate = 200;

        #endregion

        #region Frontier Shuttles
        public const float MolesCellShuttle = 2500f;
        #endregion
    }

    /// <summary>
    ///     Gases to Ids. Keep these updated with the prototypes!
    /// </summary>
    [Serializable, NetSerializable]
    public enum Gas : sbyte
    {
        Oxygen = 0,
        Nitrogen = 1,
        CarbonDioxide = 2,
        Plasma = 3,
        Tritium = 4,
        WaterVapor = 5,
        Ammonia = 6,
        NitrousOxide = 7,
        Frezon = 8,
        BZ = 9, //Funky/Goob
        Healium = 10, //Funky/Goob
        Nitrium = 11, //Funky/Goob
		Pluoxium = 12, //Funky/Goob
       reagent_name_blood = 13,

reagent_name_insect_blood = 14,

reagent_name_slime = 15,

reagent_name_sap = 16,

reagent_name_hemocyanin_blood = 17,

reagent_name_ammonia_blood = 18,

reagent_name_zombie_blood = 19,

reagent_name_ichor = 20,

reagent_name_fat = 21,

reagent_name_vomit = 22,

reagent_name_grey_matter = 23,

reagent_name_e_z_nutrient = 24,

reagent_name_left4_zed = 25,

reagent_name_pest_killer = 26,

reagent_name_plant_b_gone = 27,

reagent_name_robust_harvest = 28,

reagent_name_sedin = 29,

reagent_name_weed_killer = 30,

reagent_name_ammonia = 31,

reagent_name_diethylamine = 32,

reagent_name_acetone = 33,

reagent_name_phenol = 34,

reagent_name_charcoal = 35,

reagent_name_ash = 36,

reagent_name_sodium_carbonate = 37,

reagent_name_artifexium = 38,

reagent_name_benzene = 39,

reagent_name_hydroxide = 40,

reagent_name_sodium_hydroxide = 41,

reagent_name_fersilicite = 42,

reagent_name_sodium_polyacrylate = 43,

reagent_name_cellulose = 44,

reagent_name_rororium = 45,

reagent_name_bleach = 46,

reagent_name_space_cleaner = 47,

reagent_name_soap = 48,

reagent_name_space_lube = 49,

reagent_name_space_glue = 50,

reagent_name_aluminium = 51,

reagent_name_carbon = 52,

reagent_name_chlorine = 53,

reagent_name_copper = 54,

reagent_name_fluorine = 55,

reagent_name_gold = 56,

reagent_name_hydrogen = 57,

reagent_name_iodine = 58,

reagent_name_iron = 59,

reagent_name_lithium = 60,

reagent_name_mercury = 61,

reagent_name_potassium = 62,

reagent_name_phosphorus = 63,

reagent_name_radium = 64,

reagent_name_silicon = 65,

reagent_name_silver = 66,

reagent_name_sulfur = 67,

reagent_name_sodium = 68,

reagent_name_uranium = 69,

reagent_name_zinc = 70,

reagent_name_carpetium = 71,

reagent_name_fiber = 72,

reagent_name_buzzochloric_bees = 73,

reagent_name_ground_bee = 74,

reagent_name_saxoite = 75,

reagent_name_licoxide = 76,

reagent_name_razorium = 77,

reagent_name_fresium = 78,

reagent_name_laughter = 79,

reagent_name_weh = 80,

reagent_name_hew = 81,

reagent_name_oxygen = 82,

reagent_name_plasma = 83,

reagent_name_tritium = 84,

reagent_name_carbon_dioxide = 85,

reagent_name_nitrogen = 86,

reagent_name_nitrous_oxide = 87,

reagent_name_frezon = 88,

reagent_name_cryptobiolin = 89,

reagent_name_dylovene = 90,

reagent_name_diphenhydramine = 91,

reagent_name_ethylredoxrazine = 92,

reagent_name_arithrazine = 93,

reagent_name_bicaridine = 94,

reagent_name_cryoxadone = 95,

reagent_name_doxarubixadone = 96,

reagent_name_dermaline = 97,

reagent_name_dexalin = 98,

reagent_name_dexalin_plus = 99,

reagent_name_epinephrine = 100,

reagent_name_hyronalin = 101,

reagent_name_ipecac = 102,

reagent_name_inaprovaline = 103,

reagent_name_kelotane = 104,

reagent_name_leporazine = 105,

reagent_name_barozine = 106,

reagent_name_phalanximine = 107,

reagent_name_polypyrylium_oligomers = 108,

reagent_name_ambuzol = 109,

reagent_name_ambuzol_plus = 110,

reagent_name_pulped_banana_peel = 111,

reagent_name_saline = 112,

reagent_name_siderlac = 113,

reagent_name_stellibinin = 114,

reagent_name_synaptizine = 115,

reagent_name_tranexamic_acid = 116,

reagent_name_tricordrazine = 117,

reagent_name_lipozine = 118,

reagent_name_omnizine = 119,

reagent_name_ultravasculine = 120,

reagent_name_oculine = 121,

reagent_name_cognizine = 122,

reagent_name_ethyloxyephedrine = 123,

reagent_name_diphenylmethylamine = 124,

reagent_name_sigynate = 125,

reagent_name_lacerinol = 126,

reagent_name_puncturase = 127,

reagent_name_bruizine = -128,

reagent_name_holywater = -127,

reagent_name_pyrazine = -126,

reagent_name_insuzine = -125,

reagent_name_opporozidone = -124,

reagent_name_necrosol = -123,

reagent_name_necrosol_1 = -122,

reagent_name_necrosol_2 = -121,
reagent_name_psicodine = -120,

reagent_name_potassium_iodide = -119,

reagent_name_haloperidol = -118,

reagent_name_desoxyephedrine = -117,

reagent_name_ephedrine = -116,

reagent_name_stimulants = -115,

reagent_name_thc = -114,

reagent_name_nicotine = -113,

reagent_name_impedrezene = -112,

reagent_name_space_drugs = -111,

reagent_name_bananadine = -110,

reagent_name_nocturine = -109,

reagent_name_mute_toxin = -108,

reagent_name_norepinephric_acid = -107,

reagent_name_tear_gas = -106,

reagent_name_happiness = -105,

reagent_name_thermite_1 = -104,
reagent_name_thermite = -103,

reagent_name_napalm = -102,

reagent_name_phlogiston = -101,

reagent_name_chlorine_trifluoride = -100,

reagent_name_foaming_agent = -99,

reagent_name_welding_fuel = -98,

reagent_name_fluorosurfactant = -97,

reagent_name_toxin = -96,

reagent_name_carpotoxin = -95,

reagent_name_chloral_hydrate = -94,

reagent_name_gastrotoxin = -93,

reagent_name_mold = -92,

reagent_name_polytrinic_acid = -91,

reagent_name_ferrochromic_acid = -90,

reagent_name_fluorosulfuric_acid = -89,

reagent_name_sulfuric_acid = -88,

reagent_name_unstable_mutagen = -87,

reagent_name_heartbreaker_toxin = -86,

reagent_name_lexorin = -85,

reagent_name_mindbreaker_toxin = -84,

reagent_name_histamine = -83,

reagent_name_theobromine = -82,

reagent_name_amatoxin = -81,

reagent_name_vent_crud = -80,

reagent_name_romerol = -79,

reagent_name_uncooked_animal_proteins = -78,

reagent_name_allicin = -77,

reagent_name_pax = -76,

reagent_name_honk = -75,

reagent_name_lead = -74,

reagent_name_bungotoxin = -73,

reagent_name_vestine = -72,

reagent_name_tazinide = -71,

reagent_name_lipolicide = -70,

reagent_name_mechanotoxin = -69,
    }
}


