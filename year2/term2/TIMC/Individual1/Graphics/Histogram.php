<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
 @session_start();
    $contFreqTable = $_SESSION["ContFreqTable"];

    $width = 350;

    $minRectWidth = 20;
    //useful values
    $numbersCount = count($contFreqTable);

    $height = 400;

//calculate image height
    if (($height - 10) / $numbersCount < $minRectWidth)
        $height = $minRectWidth * $numbersCount + 10;

    $image = imagecreatetruecolor($width, $height);

    $backgroundColor = imagecolorallocate($image, 237, 255, 234);
    $barBorderColor = imagecolorallocate($image, 189, 164, 245);
    $barFilledColor = imagecolorallocate($image, 219, 225, 255);    
    $barCentralLineColor = imagecolorallocate($image, 237, 239, 255);
    $textColor = imagecolorallocate($image, 82, 76, 255);

    //fill background
    imagefill($image, 0, 0, $backgroundColor);

    $maxColumnHeight = $width - 10;
    $columnsWidth = ($height - 10) / $numbersCount;

    $maxElement = max(array_values($contFreqTable));
    $freqKeys = array_keys($contFreqTable);

    //imagesetthickness($image, 2);

    $d = 0.0;
    $index = 0;
    //draw all columns
    foreach($contFreqTable as $key=>$value)
    {
        $d = $value / $maxElement;
        $currentColumnHeight = $d * $maxColumnHeight;

        $x1 = 5;
        $x2 = $currentColumnHeight + $x1;

        $y1 = 5 + $index*$columnsWidth;
        $y2 = $y1 + $columnsWidth - 5;

        $centralY = ($y1 + $y2) >> 1;

        imagefilledrectangle($image, $x1, $y1, $x2, $y2, $barFilledColor);        
        imagerectangle($image, $x1, $y1, $x2, $y2, $barBorderColor);

        //imagedashedline($image, $x1, $centralY, $x2, $centralY, $barBorderColor);
        imageline($image, $x1 + 2, $centralY, $x2 - 2, $centralY, $barCentralLineColor);
        imagefilledellipse($image, $x1, $centralY, 4, 4, $barBorderColor);

        imagestring($image, 2, $x1 + 4, $centralY, $key, $textColor);
        imagestring($image, 3, $x2 - strlen($value)*7 - 3, $centralY, $value, $textColor);

        ++$index;
    }

    
//send image to browser
    header("Content-type: image/png");
    $rotated = imagerotate($image, 90, $backgroundColor);
    imagepng($rotated);
?>
