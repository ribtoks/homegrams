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
    $betweenLineWidth = 15;
    $numbersCount = count($freqTable);

//calculate image height
    if (($numbersCount + 1) * $betweenLineWidth <= 400)
    {
        $height = 400;
        $betweenLineWidth = $height / ($numbersCount + 1);
    }
    else
    {
        $height = ($numbersCount + 1) * $betweenLineWidth;        
    }

    $image = imagecreatetruecolor($width, $height);
    imageantialias($image, true);

    $backgroundColor = imagecolorallocate($image, 239, 251, 255);
    $barColor = imagecolorallocate($image, 97, 93, 179);
    $lightColor = imagecolorallocate($image, 207, 202, 202);

    //fill background
    imagefill($image, 0, 0, $backgroundColor);

    $d1 = 0.0;
    $d2 = 0.0;

    //---------------------------------------------------
    $freqKeys = array_keys($freqTable);

    $maxKey = max($freqKeys);
    $minKey = min($freqKeys);

    $maxLengthKey = strlen($maxKey);
    if (strlen($minKey) > $maxLengthKey)
        $maxLengthKey = strlen($minKey);
    //---------------------------------------------------

    $maxColumnHeight = $width - 35 - $maxLengthKey*7;
    $index = 0;
    $margin_left = $maxLengthKey*7 + 5;

    //---------------------------------------------------
    $freqValues = array_values($freqTable);
    $maxElement = max($freqValues);
    $minElement = min($freqValues);

    $minusValue = ($minElement / $maxElement) * $maxColumnHeight;
//imagestring($image, 3, 50, 50, count($freqValues), $barColor);
    //---------------------------------------------------

    $countFreqValues = count($freqValues);

    //draw lines between every two points
    for ($i = 0; $i < $countFreqValues - 1; $i++)
    {        
        $d1 = $freqValues[$i] / $maxElement;
        $d2 = $freqValues[$i + 1] / $maxElement;

        $x1 = $margin_left  +  ($d1 * $maxColumnHeight) - $minusValue;
        $y1 = ($index + 1) * $betweenLineWidth;

        $x2 =  $margin_left +  ($d2 * $maxColumnHeight) - $minusValue;
        $y2 = ($index + 2) * $betweenLineWidth;

//draw line
        imageline($image, $x1, $y1, $x2, $y2, $barColor);

        imageline($image, 5, $y1, $x1, $y1, $lightColor);

        if ($i == $countFreqValues - 2)
        {
            imageline($image, 5, $y2, $x2, $y2, $lightColor);

            //type integer value
       imagestring($image, 2, 5, $y2, $freqKeys[$i + 1], $barColor);
        }

       //type integer value
       imagestring($image, 2, 5, $y1, $freqKeys[$i], $barColor);
       
        $index++;
    }

//send image to browser
    header("Content-type: image/png");
    $rotated = imagerotate($image, 90, $backgroundColor);
    
    $index = 0;

    $barColor =  imagecolorallocate($rotated, 97, 93, 179);

    for ($i = 0; $i < count($freqValues) - 1; $i++)
    {
        $d1 = $freqValues[$i] / $maxElement;
        $d2 = $freqValues[$i + 1] / $maxElement;

        $x1 = $margin_left +  ($d1 * $maxColumnHeight) - $minusValue;
        $y1 = ($index + 1) * $betweenLineWidth;

        $x2 =  $margin_left +  ($d2 * $maxColumnHeight) - $minusValue;
        $y2 = ($index + 2) * $betweenLineWidth;

        //if ($y1 !== $y2)
            imagestring($rotated, 3, $y1 - 5, $width - $x1 - 14, $freqValues[$i], $barColor);

        if ($i == count($freqValues) - 2)
            imagestring($rotated, 3, $y2 - 5, $width - $x2 - 14, $freqValues[$i + 1], $barColor);

        $index++;
    }

    imagepng($rotated);
  ?>
