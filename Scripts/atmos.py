import os
from pathlib import Path
from PIL import Image
from transforms import RGBTransform
import shutil
import matplotlib.colors
#get path
pth = os.getcwd()
pth = pth.partition("Scripts")[0]
base = pth
pth += "Resources/Prototypes/Reagents"
pth = Path(pth)
colors = list()
names = list()
#go to gases
os.chdir(pth)
if not Path.exists(pth):
    os._exit

num = 13 #id num

#main loop: for every file...
for file in os.listdir(pth):
    fil = file.partition(".")[0]
    car = ""
    if not Path.exists(Path(fil+"_Gas.yml")):
        car = "x"
    else:
        continue
    if Path.is_dir(Path(file)):
        continue
    f = open(fil+"_Gas.yml",car)
    nf = open(file)
    b = nf.read()
    bu = b.split("-",1)[1]
    buff = bu.split("- type: reagent") # get data block

    for buf in buff:
        
        id = "id: "+str(num)+"\n"
        name = "name: "
        specificHeat = "specificHeat: "
        heatCapacityRatio = "heatCapacityRatio: "
        molarMass = "molarMass: 1\n"
        color = "color: "
        reagent = "reagent: "
        sprite= "gasOverlaySprite: /Textures/_DV/Effects/atmospherics.rsi\n"
        state = "gasOverlayState: "

        buf = buf.partition("id: ")[2]
        dat = buf.partition("\n")
        reagent += ""+dat[0]+"" + "\n"
        
        buf = buf.partition("name: ")[2]
        dat = buf.partition("\n")
        d = dat[0].replace("reagent-name-","").replace("-","_")
        name += ""+d+"" + "\n"
        if not name.find("_"):
            continue
        print(d+" = "+str(num)+",\n")
        names.append(d)
        state += d+"_gas \n\n"
        if buf.find("metallic"):
            specificHeat += "0.5" + "\n" #https://www.engineersedge.com/materials/specific_heat_capacity_of_metals_13259.html, Btu/lb-C is around that number ehhh
            heatCapacityRatio += "1.0"+ "\n" #https://chem.libretexts.org/Bookshelves/Physical_and_Theoretical_Chemistry_Textbook_Maps/Thermodynamics_and_Chemical_Equilibrium_(Ellgen)/07%3A_State_Functions_and_The_First_Law/7.14%3A_Heat_Capacities_of_Solids-_the_Law_of_Dulong_and_Petit cp/cv ~= 1
        else:
            specificHeat += "1.0" + "\n" #default
            heatCapacityRatio += "1.4" + "\n" #default


        buf = buf.partition("color: ")[2]
        dat = buf.partition("\n")
        color += dat[0].replace("\"","").replace("#","") + "\n" #remove extra
        colors.append(dat[0].replace("\"","").replace("#",""))

        f.write("- type: gas\n"+"  "+id+"  "+name+"  "+specificHeat+"  "+heatCapacityRatio+"  "+molarMass+"  "+color+"  "+reagent+"  "+sprite+"  "+state) #write it all!
        num = num +1
        f.flush()
    f.close()
    nf.close()

text = Path(base + "Resources/Textures/_DV/Effects/atmospherics.rsi")
os.chdir(text)
pos = 0
for n in names:
    if (str(colors[pos]).endswith("#") or (not str(colors[pos])[-1].isdigit)):
        pos += 1
        continue
    newName = n+"_gas.png"
    shutil.copy(Path("water_vapor.png"),Path(newName))
    png = Image.open(newName)
    png = png.convert('RGBA')
    col = matplotlib.colors.to_rgb("#"+colors[pos])
    print(colors[pos]+" : "+str(col[0])+str(col[1])+str(col[2]))
    c = RGBTransform().mix_with((col[0]*256,col[1]*256,col[2]*256),factor=.30).applied_to(png) #https://stackoverflow.com/questions/32578346/how-to-change-color-of-image-using-python
    c.save(newName)
    pos += 1