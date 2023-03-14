rm -f ./output/$1.png
for img in *.png
do 
   #convert wumpus_base.png $img -composite ./output/$img
    convert *.jpg -resize 128x128 -gravity center -extent 128x128 ./output/*.png
done
#convert -append ./output/*$1*.png ./output/$1.png
#rm -f ./output/*.png
