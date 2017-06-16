<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

require_once 'DiscretePlotter.php';
require_once 'Resources.php';

/**
 * Description of ContinuousPlotter
 *
 * @author taras
 */
class ContinuousPlotter
{
    private $dPlotter;
    private $step = 0.01;

    private $arrayX;
    private $arraysY;

    public function  __construct($leftBound, $rightBound, $funcArray,
            $globalTitle,
            $backgroundColorString, $graphicsColorStrings,
            $width, $height, $graphicsCountOnImage = 1, $step = 0.001,
            $xTitle = null, $yTitle = null)
    {
        $step = max(array($step, ($rightBound - $leftBound) / 10000));

        $this->step = $step;
        $this->arrayX = range($leftBound, $rightBound, $step);
        $this->arraysY = array();

        for ($i = 0, $count = count($funcArray); $i < $count; ++$i)
        {
            $func = $funcArray[$i];
            $this->arraysY[] = array_map($func, $this->arrayX);
        }

        $this->dPlotter = new DiscretePlotter($this->arrayX, $this->arraysY,
            $globalTitle,
            $backgroundColorString, $graphicsColorStrings,
            $width, $height, $graphicsCountOnImage, $xTitle, $yTitle);

        $this->dPlotter->drawMarkers = false;
    }

    public function ProcessGraphics()
    {
        $this->dPlotter->ProcessGraphics();
    }

    public function OutGraphics($index)
    {
        $this->dPlotter->OutGraphics($index);
    }
}
?>
