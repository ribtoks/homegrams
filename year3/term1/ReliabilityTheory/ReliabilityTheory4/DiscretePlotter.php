<?php
/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

include_once 'Resources.php';


/**
 * Description of DiscretePlotter
 *
 * @author taras
 */
class DiscretePlotter
{
    // offsets of real graphic area on picture
    //private $offsetX;
    //private $offsetY;

    // titles of graphic and of coordinate axis
    private $title;

    // next 2 fields are not used
    // in this version
    private $xTitle;
    private $yTitle;

    // arrays of points
    private $arrayX;
    private $arraysY;

    // number of graphics on one picture
    private $graphicsCountOnImage;

    // colors of each graphic lines
    private $graphicsColors;

    // background color
    private $background;

    // arrray of images with graphics
    private $images;
    private $width;
    private $height;

    // max(arrayX) - min(arrayX)
    private $xWidth;

    // number of images, that would be drawn
    private $imagesCount;

    // maximum length of any Y data
    private $theMaxMaxLength;

    private $markerSize = 4;

    private $minXvalue;
    private $maxXvalue;

    public $drawMarkers = true;


    public function __construct($arrayX, $arraysY, $globalTitle,
                                    $backgroundColorString, $graphicsColorStrings,
                                    $width, $height,
                                    $graphicsCountOnImage = 1,
                                    $xTitle = null, $yTitle = null)
    {
        $this->width = $width;
        $this->height = $height;

        $this->title = $globalTitle;
        $this->xTitle = $xTitle;
        $this->yTitle = $yTitle;

        $this->arrayX = $arrayX;
        $this->arraysY = $arraysY;

        $this->minXvalue = min($arrayX);
        $this->maxXvalue = max($arrayX);
        $this->xWidth = $this->maxXvalue - $this->minXvalue;

        $countData = count($arraysY);

        $this->imagesCount = ceil(count($arraysY) / $graphicsCountOnImage);

        if ($countData >= $graphicsCountOnImage)
            $this->graphicsCountOnImage = $graphicsCountOnImage;
        else
            $this->graphicsCountOnImage = $countData;

        $this->width = $width;
        $this->height = $height;

        $this->graphicsColors = $graphicsColorStrings;
        $this->background = $backgroundColorString;

        $this->images = array();
    }

    public function ProcessGraphics()
    {
        global $Color;

        $dataCount = count($this->arraysY);

        // pick colors and marker types for each graphic
        for ($i = 0; $i < $dataCount; ++$i)
        {
            $markers[] = rand(0, 2);
            $colors[] = $this->graphicsColors[array_rand($this->graphicsColors)];
        }

        $minValue = floor($this->minXvalue);
        $maxValue = ceil($this->maxXvalue);

        if (strlen($minValue) > strlen($maxValue))
            $maxLength = strlen($minValue);
        else
            $maxLength = strlen($maxValue);

        $theLongest = array();

        // find all widthes of data
        for ($i = 0; $i < $dataCount; ++$i)
        {
            $widthsMax[] = ceil(max($this->arraysY[$i]));
            $widthsMin[] = floor(min($this->arraysY[$i]));
        }

        $arrMax = array();
        $arrMin = array();

        for ($i = 0; $i < $dataCount; ++$i)
        {
            $arrMax[] = $widthsMax[$i];
            $arrMin[] = $widthsMin[$i];

            if ((($i + 1) % $this->graphicsCountOnImage) == 0)
            {
                $maxElement = ceil(max($arrMax));
                $minElement = floor(min($arrMin));

                $bestWidths[] = $maxElement - $minElement;
                $bestMaximums[] = $maxElement;
                $arrMax = array();
                $arrMin = array();

                $theLongest[] = strlen($maxElement);
                $theLongest[] = strlen($minElement);
            }
        }
        // add last data if needed
        if (($dataCount % $this->graphicsCountOnImage) != 0)
        {
            $maxElement = ceil(max($arrMax));
            $minElement = floor(min($arrMin));

            $bestWidths[] = $maxElement - $minElement;

            $theLongest[] = strlen($maxElement);
            $theLongest[] = strlen($minElement);
        }

        // find the longest number
        $this->theMaxMaxLength = max($theLongest);

        $savedImages = 0;
        $currWidthIndex = 0;
        // loop for drawing graphics
        for ($i = 0; $i < $dataCount; ++$i)
        {
            $currentBestWidth = $bestWidths[$currWidthIndex];

            // if current iteration divides graphics count
            // it means, that we must save current image
            // and create next image
            if (($i % $this->graphicsCountOnImage) == 0)
            {
                if (isset($image))
                {
                    $this->SaveOneGraphic($image, $font, $graphicColor, $currentBestWidth);
                    ++$savedImages;
                    ++$currWidthIndex;

                    $currentBestWidth = $bestWidths[$currWidthIndex];
                }

                $image = imagecreatetruecolor($this->width, $this->height);

                $grColor = $Color[$this->background];
                $background = imagecolorallocate($image,
                        $grColor['r'], $grColor['g'], $grColor['b']);

                imagefill($image, 0, 0, $background);

                $font = 3;

                $offsetX = imagefontheight($font) + 6;
                $offsetY = 2*imagefontheight($font) + 8;

                $width = $this->width - $offsetX;
                $height = $this->height - $offsetY -
                    ($maxLength + 2) * imagefontwidth($font);

                $this->ProcessGrid($image, $font, $background,
                    $graphicColor, $currentBestWidth);

                // antialias works only with true color images
                // Setstyle and Antialias don't go together
                imageantialias($image, true);
            }

            $grColor = $Color[ $colors[$i] ];
            $graphicColor = imagecolorallocate($image,
                    $grColor['r'], $grColor['g'], $grColor['b']);

            // than just draw graphic on current image

            $offsetY = 2*imagefontheight($font) + 8;

            $maxElement = ceil(max($this->arraysY[$i]));
            $currWidth = $maxElement - floor(min($this->arraysY[$i]));
            $currMiniOffset = $bestMaximums[$currWidthIndex] - $maxElement;

            // offset of current graphic
            $offsetY += ($currMiniOffset / $currentBestWidth)*$height;
            $graphicHeight = ($currWidth / $currentBestWidth)*$height;

            $delta = ($this->theMaxMaxLength + 1)*imagefontwidth($font) + 2;
            // minus length of the longest number
            $width = $this->width - $offsetX;

            if ($this->drawMarkers)
                $width -= $delta;

            $this->PreprocessOneGraphic($image, $this->arrayX, $this->arraysY[$i],
                $graphicColor, $background,
                $offsetX, $offsetY, $width, $graphicHeight, $markers[$i]);
        }

        if ($this->imagesCount > $savedImages)
            // last image is not saved
            if (isset($image))
            {
                $this->SaveOneGraphic($image, $font, $graphicColor, $currentBestWidth);
            }
    }

    // saves preprocessed graphic to object images
    private function SaveOneGraphic(&$image, $font, $graphicColor, $currentBestWidth)
    {
        // print global caption of graphic
        $x = ($this->width / 2) -
                        (strlen($this->title)*imagefontwidth($font) / 2);
        $y = 6;

        imagestring($image, $font, $x, $y, $this->title, $graphicColor);

        /*
        $x = 3;
        $y = ($this->height / 2) +
                        (strlen($this->yTitle)*imagefontwidth($font) / 2);
        imagestringup($image, $font, $x, $y, $this->yTitle, $graphicColor);
        */


        $this->images[] = imagecreatetruecolor($this->width, $this->height);
        $last = count($this->images) - 1;

        imageantialias($this->images[$last], true);

        // save graphic
        imagecopy($this->images[$last], $image,
                0, 0, // destination coords
                0, 0, // source coords
                $this->width, $this->height);

        // destroy resources
        imagedestroy($image);
    }

    public function OutGraphics($index)
    {
        header("Content-type: image/png");
        imagepng($this->images[$index]);
    }

    private function PreprocessOneGraphic(&$image, $xData, $yData,
        $graphicColor, $background, $offsetX, $offsetY, $width, $height, $markerCode)
    {
        $xCount = count($xData);
        $yCount = count($yData);

        if ($xCount != $yCount)
            die("Different sizes of X and Y points arrays.");

        global $Color;

        // sort arrays in order to draw it correctly
        $sorted = array_combine($xData, $yData);
        ksort($sorted);

        $yData = array_values($sorted);
        $xData = array_keys($sorted);

        // xValues are sorted
        $minX = floor($xData[0]);
        $maxX = ceil($xData[$xCount - 1]);

        $minY = min($yData);
        $maxY = max($yData);

        $minY = floor($minY);
        $maxY = ceil($maxY);

        $stepX = $width / ($maxX - $minX);
        $stepY = $height / ($maxY - $minY);

        $markerSize = $this->markerSize;


        // offset must have space for signings
        $font = 2;

        $charWidth = imagefontwidth($font);
        $charHeight = imagefontheight($font);

        // main loop of drawing all lines
        for ($i = 0; $i < $xCount - 1; ++$i)
        {
            $x1 = $xData[$i];
            $y1 = $yData[$i];

            $x2 = $xData[$i + 1];
            $y2 = $yData[$i + 1];

            //---------------------------

            // find real coordinates...

            $x1 = ($x1 - $minX)*$stepX + $offsetX;
            $x2 = ($x2 - $minX)*$stepX + $offsetX;

            $y1 = ($maxY - $y1)*$stepY + $offsetY;
            $y2 = ($maxY - $y2)*$stepY + $offsetY;

            //---------------------------

            imageline($image, $x1, $y1, $x2, $y2, $graphicColor);
        }

        if ($this->drawMarkers)
        {
            // draw markers
            // sign values
            // draw lines to X axis
            for ($i = 0; $i < $xCount; ++$i)
            {
                $cx = ($xData[$i] - $minX)*$stepX + $offsetX;
                $cy = ($maxY - $yData[$i])*$stepY + $offsetY;

                $value = $markerSize / 2;

                // draw line to X axis
                //imagedashedline($image, $cx, $cy, $cx, $height + $offsetY, $grayColor);

                //$x = $cx;
                //$y = $height + $offsetY + imagefontwidth($font)*(strlen($xData[$i]) + 1);
                //imagestringup($image, $font, $x, $y, strval($xData[$i]), $graphicColor);

                switch ($markerCode)
                {
                    case 0:
                        // circle
                        imagefilledellipse($image, $cx, $cy,
                            $markerSize, $markerSize, $graphicColor);
                        break;

                    case 1:
                        // square
                        imagefilledrectangle($image, $cx - $value, $cy - $value,
                            $cx + $value, $cy + $value, $graphicColor);
                        break;

                    case 2:
                        // triangle
                        $points = array($cx, $cy - $value, $cx + $value, $cy + $value,
                                        $cx - $value, $cy + $value);
                        imagefilledpolygon($image, $points, count($points) / 2, $graphicColor);
                        break;

                    default:
                        break;
                }

                // sign value
                $stringValue = $yData[$i];
                $rectWidth = strlen($stringValue)*$charWidth + 2;
                $rectHeight = $charHeight + 2;

                $x = $cx + 1;
                $y = $cy - $value - 1 - $rectHeight;

                imagefilledrectangle($image, $x, $y,
                    $x + $rectWidth, $y + $rectHeight, $background);

                imagestring($image, $font, $x + 1, $y + 1, $stringValue, $graphicColor);
            }
            //if end
        }
    }

    // provides initializations needed for
    // drawing a grid
    private function ProcessGrid(&$image, $font, $background,
        $graphicColor, $currentBestWidth)
    {
        $offsetX = imagefontheight($font) + 6;
        $offsetY = 2*imagefontheight($font) + 8;

        $width = $this->width - $offsetX;
        $height = $this->height - $offsetY;

        $delta = ($this->theMaxMaxLength + 1)*imagefontwidth($font) + 2;

        $stepXpx = $width / $this->xWidth;
        if ($stepXpx < 30 || $stepXpx > ($width / 4))
            $stepXpx = 30;

        //echo "curr best width - $currentBestWidth<br />";

        $stepYpx = $height / $currentBestWidth;
        if ($stepYpx < 30 || $stepYpx > ($height / 4))
            $stepYpx = 30;

        $stepX = ($stepXpx * $this->xWidth) / ($width - $delta);

        $stepY = 0;

        $this->DrawGrid($image, $offsetX, $offsetY, $background,
            $graphicColor, $font, intval($stepXpx), intval($stepYpx),
            round($stepX), round($stepY), $width, $height);
    }

    // draws grid on graphic area
    private function DrawGrid(&$image, $offsetX, $offsetY,
        $background, $graphicColor, $font,
        $stepXpx, $stepYpx,
        $stepX, $stepY,
        $width, $height)
    {
        global $Color;

        // setstyle and antialias don't go together
        //imageantialias($image, false);

        $grColor = $Color['gainsboro'];
        $grayColor = imagecolorallocate($image,
            $grColor['r'], $grColor['g'], $grColor['b']);

        $min = min($this->arrayX);

        // Define our style: First 4 pixels is white and the
        // next 4 is transparent. This creates the dashed line effect
        $style = array(
                $grayColor,
                $grayColor,
                $grayColor,
                $grayColor,
                $background,
                $background,
                $background,
                $background,
                $background
                );

        imagesetstyle($image, $style);

        // vertical lines
        for ($i = 0; $i < $width; $i += $stepXpx)
        {
            $x1 = $offsetX + $i;
            $y1 = $offsetY;

            $y2 = $height + $y1;
            $x2 = $x1;

            imageline($image, $x1, $y1, $x2, $y2, IMG_COLOR_STYLED);
        }

        // horisontal lines
        for ($i = 0; $i < $height; $i += $stepYpx)
        {
            $x1 = $offsetX;
            $y1 = $offsetY + $i;

            $x2 = $width + $x1;
            $y2 = $y1;

            imageline($image, $x1, $y1, $x2, $y2, IMG_COLOR_STYLED);
        }

        // sign values
        for ($i = 0, $xValue = $min; $i < $width; $i += $stepXpx, $xValue += $stepX)
        {
            $x1 = $offsetX + $i;

            // sign lines
            $x = $x1;
            $y = $height + imagefontwidth($font)*(strlen(intval($xValue))) + 6;

            //echo $xValue;
            //echo "<br />";

            imagestringup($image, $font, $x, $y, intval($xValue), $graphicColor);
        }
    }
}
?>