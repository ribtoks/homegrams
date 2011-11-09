<?php

session_start();
$freqTable = $_SESSION["FrTable"];

//height of a picture
$height = 400;

//width of a picture
$width = 520;

$Cx = 0;
$Cy = 0;

$freqKeys = array_keys($freqTable);

 $minValue = min($freqKeys);
 $maxValue = max($freqKeys);

//a simple hack that is not useful anymore
 if ($maxValue - $minValue > 1000)
    $width += 100;

//create image for graphic
$image = imagecreate($width, $height);
imageantialias($image, true);

$backgroundColor = imagecolorallocate($image, 246, 255, 244);
$barColor = imagecolorallocate($image, 97, 93, 179);

$lightColor = imagecolorallocate($image, 207, 202, 202);

 //fill background
 imagefill($image, 0, 0, $backgroundColor);

//-----------------------------------------------------------------
//Drawing coordinate axes
//----------------------------------------------------------------
$maxLessZero = false;

 if ($maxValue < 0)
 {
    $maxValue = 1;
    $maxLessZero = true;
 }

 //calculate chunk of X-axis with length 1
 if ($minValue < 0)
 {
    $XAxisCellWidth = $width / ($maxValue - $minValue + 2);
    $minValue--;
 }
else
{
     $XAxisCellWidth = $width / ($maxValue + 2);
     $minValue = -1;
}

//coordinates of axes start
$Cy = $height - 15;
$Cx = abs($minValue) * $XAxisCellWidth + 2;

//Y-axis
 imageline($image, 2, $Cy, $width - 2, $Cy, $lightColor);

 $index = 0;
 
 for ($i = $minValue; $i <= $maxValue + 1; $i++)
 {
     if (array_key_exists($i, $freqTable))
     {
        imagedashedline($image, $index * $XAxisCellWidth + 2, 2,
            $index * $XAxisCellWidth + 2, $height - 2, $lightColor);
     
        imagestring($image, 1, $index * $XAxisCellWidth + 4, $Cy + 2, $i, $barColor);
     }

     $index++;
 }

//X-axis
 imageline($image, $Cx, 2, $Cx, $height - 2, $lightColor);

  if ($maxLessZero)
    imagestring($image, 1, $Cx + 2, $Cy + 2, "0", $barColor);

//--------------------------------------------------------------------------------

 //---------------------------------------------------------------------------
 //Drawing Graphic
 //--------------------------------------------------------------------------

 //position of '1' value on Y-axis
 $OneY = 10;

 imageline($image, $Cx - 2, $OneY, $Cx + 3 , $OneY, $barColor);
 imagestring($image, 3, $Cx - 10, $OneY, "1", $barColor);

 imagesetthickness($image, 2);

 //length of [0, 1]
 $OneDist = $Cy - $OneY;

 $freqValues = array_values($freqTable);

 $n = array_sum($freqValues);
 $k = 0;

$tempX2 =  $Cx + $freqKeys[0]*$XAxisCellWidth - 2;

imageline($image, 0, $Cy, $tempX2, $Cy, $barColor);

imageline($image, $tempX2 - 2, $Cy - 2, $tempX2, $Cy, $barColor);
imageline($image, $tempX2 - 2, $Cy + 2, $tempX2, $Cy, $barColor);

 for ($i = 0, $countFreqKeys = count($freqKeys); $i < $countFreqKeys -1; $i++)
 {     
     $k += $freqValues[$i];

     $x1 = $Cx + $freqKeys[$i]*$XAxisCellWidth;
     
     $y = $k / $n;
     $y12 = $Cy - ($y * $OneDist);

     $x2 = $Cx + $freqKeys[$i + 1]*$XAxisCellWidth;
     
     imagestring($image, 1, $x1 + 3, $y12 - 10, substr($y, 0, 5), $barColor);

     imageline($image, $x1, $y12, $x2 - 2, $y12, $barColor);

     imageline($image, $x2 - 2, $y12 - 2, $x2, $y12, $barColor);
     imageline($image, $x2 - 2, $y12 + 2, $x2, $y12, $barColor);
 }

 imagesetthickness($image, 1);
//draw last line > 1
 imageline($image, 2, $OneY, $width, $OneY, $lightColor);

imagesetthickness($image, 2);
 imageline($image, $Cx + $freqKeys[count($freqKeys) - 1]*$XAxisCellWidth, $OneY,
     $width, $OneY, $barColor);

 imagepng($image);
?>
