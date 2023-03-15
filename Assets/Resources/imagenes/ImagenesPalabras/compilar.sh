#rm -f ./output/$1.png
for img in *.png
do 
    convert *.jpg -resize 128x128 -gravity center -extent 128x128 *.png
    read -p "Todas las imagenes convertidas " -n1 -s
    exit 0
done
#convert -append ./output/*$1*.png ./output/$1.png
#rm -f ./output/*.png
