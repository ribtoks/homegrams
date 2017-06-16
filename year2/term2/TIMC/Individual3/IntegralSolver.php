<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
require_once 'IntegralCalculator.php';
require_once 'GammaFunc.php';

/**
 * Description of IntegralSolver
 *
 * @author taras
 */
class IntegralSolver
{
    private $integralCalc;
    private $epsilon = 0.00001;
    private $densityFunc;
    private $df1;
    private $df2;

    public function  __construct($df1, $df2, $epsilon = 0.00001)
    {
        $this->epsilon = $epsilon;
        $this->integralCalc = new IntegralCalculator($this->epsilon);
        
        $this->df1 = $df1;
        $this->df2 = $df2;

        $this->densityFunc = function($x) use ($df1, $df2)
        {
            $dfDiv = $df1 / $df2;

            $temp = GammaFunction::Calculate(($df1 + $df2) / 2);
            $temp /= GammaFunction::Calculate($df1 / 2);
            $temp /= GammaFunction::Calculate($df2 / 2);

            $temp *= pow($dfDiv, $df1 / 2);
            $temp *= pow($x, ($df1 / 2) - 1);
            $temp *= pow(1 + $dfDiv*$x, -($df1 + $df2) / 2);

            return $temp;
        };
    }

    public function SolvePhisherLower($alpha)
    {
        $df1 = $this->df2;
        $df2 = $this->df1;

        $func = function($x) use ($df1, $df2)
        {
            $dfDiv = $df1 / $df2;

            $temp = GammaFunction::Calculate(($df1 + $df2) / 2);
            $temp /= GammaFunction::Calculate($df1 / 2);
            $temp /= GammaFunction::Calculate($df2 / 2);

            $temp *= pow($dfDiv, $df1 / 2);
            $temp *= pow($x, ($df1 / 2) - 1);
            $temp *= pow(1 + $dfDiv*$x, -($df1 + $df2) / 2);

            return $temp;
        };

        return $this->SolveLowerBound($func, $alpha, 0, 10000);
    }

    public function SolvePhisherUpper($alpha)
    {
        return $this->SolveUpperBound($this->densityFunc, $alpha, 0, 10000);
    }

    /*
     * finds solution of transcedent integral equation
     * using binary search
    */
    private function SolveUpperBound($function, $integralSquare, $minLowerBound, $maxUpperBound)
    {
        $currentBound = 0;
        $MaxValue = $maxUpperBound * 1000;

        $epsilon = 0.000001;

        // bounds
        $lowerBound = $minLowerBound;
        $upperBound = $maxUpperBound;

        $currentBound = ($lowerBound + $upperBound) / 2.0;

        // calculate one time interval from upper bound to "infinity"
        $currentIntegral = $this->integralCalc->Calculate($upperBound, $MaxValue, $function);

        $epsilonDiv2 = $epsilon / 2.0;

        while (abs($upperBound - $lowerBound) > $epsilonDiv2)
        {
            // calculate integral just on small sub-segment
            $segmentIntegral = $this->integralCalc->Calculate($currentBound, $upperBound, $function);

            // add integral sum
            $integral = $currentIntegral + $segmentIntegral;

            // if we have result, almost equal to our alpha
            if (abs($integral - $integralSquare) < $epsilon)
                break;

            if ($integral > $integralSquare)
                $lowerBound = $currentBound;
            else
            {
                $currentIntegral = $integral;
                $upperBound = $currentBound;
            }

            // find next value
            $currentBound = ($lowerBound + $upperBound) / 2.0;
        }

        return $currentBound;
    }

    private function SolveLowerBound($function, $integralSquare, $minLowerBound, $maxUpperBound)
    {
        $currentBound = 0;

        $epsilon = 0.000001;

        // bounds
        $lowerBound = $minLowerBound;
        $upperBound = $maxUpperBound;

        $currentBound = ($lowerBound + $upperBound) / 2.0;

        $currentIntegral = 0;// $this->integralCalc->Calculate(0, $currentBound, $function);

        $epsilonDiv20 = $epsilon / 20.0;

        while (abs($lowerBound - $upperBound) > $epsilonDiv20)
        {
            // calculate integral just on small sub-segment
            $segmentIntegral = $this->integralCalc->Calculate($lowerBound, $currentBound, $function);

            // add integral sum
            $integral = $segmentIntegral + $currentIntegral;

            // if we have result, almost equal to our alpha
            if (abs($integral - $integralSquare) < $epsilon)
                break;

            if ($integral > $integralSquare)
                $upperBound = $currentBound;
            else
            {
                $currentIntegral = $integral;
                $lowerBound = $currentBound;
            }

            // find next value
            $currentBound = ($lowerBound + $upperBound) / 2.0;
        }

        return $currentBound;
    }
}
?>
