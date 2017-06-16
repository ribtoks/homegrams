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

    public function  __construct($epsilon = 0.00001)
    {
        $this->epsilon = $epsilon;
        $this->integralCalc = new IntegralCalculator($this->epsilon);
    }

    //put your code here
    public function SolveChiSquare($df, $alpha)
    {
        $dfDiv2 = $df / 2.0;
        $sqrt2 = 1.41421356237309504880;
        $constant = pow($sqrt2, $df) * GammaFunction::Calculate($dfDiv2);

        $func = create_function('$x', 'return pow($x, '.$dfDiv2.' - 1) / (exp($x / 2.0) * '.$constant.');');
        /*
        $func = function($x) use($dfDiv2, $constant)
        {
            return pow($x, $dfDiv2 - 1) / (exp($x / 2.0) * $constant);
        };
        */
        return $this->Solve($func, $alpha, 0, 10000, 1000000);
    }

    /*
     * finds solution of transcedent integral equation
     * using binary search
    */
    public function Solve($function, $integralSquare, $minLowerBound, $maxLowerBound, $infinityValue)
    {
        $currentBound = 0;
        $MaxValue = $infinityValue;

        $epsilon = 0.000001;

        // bounds
        $lowerBound = $minLowerBound;
        $upperBound = $maxLowerBound;

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
}
?>
