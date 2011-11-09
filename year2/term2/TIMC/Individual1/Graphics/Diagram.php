<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
 //Get "posted" data
 //transform picture for easier drawing   

    @session_start();
    $freqTable = $_SESSION["FrTable"];

    $width = 350;

    //useful values
    $rectWidth = 10;
    $betweenRectWidth = 10;
    $numbersCount = count($freqTable);

//calculate image height
    if ($numbersCount * ($rectWidth + $betweenRectWidth) + $betweenRectWidth <= 400)
    {
        $height = 400;
        $betweenRectWidth = ($height - $rectWidth * $numbersCount) / ($numbersCount + 1);
    }
    else
    {
        $height = $numbersCount * ($rectWidth + $betweenRectWidth) + $betweenRectWidth;
    }

    $image = imagecreatetruecolor($width, $height);

    $backgroundColor = imagecolorallocate($image, 243, 239, 255);
    $barColor = imagecolorallocate($image, 73, 45, 255);
    $barFilledColor = imagecolorallocate($image, 117, 114, 255);

    //fill background
    imagefill($image, 0, 0, $backgroundColor);

    $d = 0.0;
    $maxColumnHeight = $width - 10;
    $index = 0;

    $maxElement = max(array_values($freqTable));
    
    $freqKeys = array_keys($freqTable);

    $maxKey = max($freqKeys);
    $minKey = min($freqKeys);

    $maxLengthKey = strlen($maxKey);
    if (strlen($minKey) > $maxLengthKey)
        $maxLengthKey = strlen($minKey);

    imageline($image, 5 + 6*$maxLengthKey, 0, 5 + 6*$maxLengthKey, $height, $barColor);

    //draw all columns
    foreach($freqTable as $key=>$value)
    {
        $d = $value / $maxElement;
        $currentColumnHeight = $d * $maxColumnHeight;

        $x1 = 5;
        $y1 = ($index + 1)*$rectWidth + $index*$betweenRectWidth;

        $x2 = $x1 + $currentColumnHeight;
        $y2 = $y1 + $rectWidth;

//draw rectangle
        if (($x2 - $x1) > 6*$maxLengthKey)
        {
            imagefilledrectangle($image, $x1, $y1, $x2, $y2, $barFilledColor);
            imagerectangle($image, $x1, $y1, $x2, $y2, $barColor);
        }
        else
        {
            imagefilledrectangle($image, $x1, $y1, $x2 + 6*$maxLengthKey, $y2, $barFilledColor);
            imagerectangle($image, $x1, $y1, $x2 + 6*$maxLengthKey, $y2, $barColor);
        }

        //type integer number
        imagestring($image, 2, $x1 + 1, $y1 - 1, $key, $backgroundColor);

        if (abs($currentColumnHeight - $maxColumnHeight) < 7*strlen($value))
        {
            //type its count
            imagestring($image, 3, $x2 - 7*strlen($value), $y1 - 1, $value, $backgroundColor);
        }
        else
            {
                 //type its count
                imagestring($image, 3, $x2 + 3, $y1 - 1, $value, $barColor);
            }

        $index++;
    }

//send image to browser
    header("Content-type: image/png");
    $rotated = imagerotate($image, 90, $backgroundColor);
    imagepng($rotated);
  ?>
