<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

//finds nth power of number a
function Power($a, $n)
{
    $x = $a;
    $r = 1;

    while($n)
    {
	if ($n & 1)
		$r *= $x;
	$n >>= 1;
	$x *= $x;
    }

    return $r;
}


 function GetCenterElement($minIndex, $maxIndex, $keyArray)
    {
         //new part
                $elementIndex = ($minIndex + $maxIndex) / 2;

                if ($elementIndex == floor($elementIndex))
                    $element = $keyArray[$elementIndex];
                else
                {
                    $elementIndexRound = ($minIndex + $maxIndex) >> 1;
                    $element = ($keyArray[$elementIndexRound] + $keyArray[$elementIndexRound + 1]) / 2;
                }

                return $element;
    }

    function EvalauteContinuousSampling($FrequencyTable, &$Step = null, &$R = null)
    {
        //initializing additional variables
        $minIndex = 0;
        $maxIndex = 0;

        $keys = array_keys($FrequencyTable);
        $arrayCount = count($FrequencyTable);

        $maxValue = max($keys);
        $minValue = min($keys);

        $r = CalculateR(array_sum($FrequencyTable));

        $sampleSwing = $maxValue - $minValue;
        $step = $sampleSwing / ($r + 1);

        $index = 1;
        $currentSum = 0;
        //end of additional variables

        //$i - index in keys
        $i = 0;
        $resultKeys = array();
        $resultValues = array();

        foreach ($FrequencyTable as $key => $value)
        {
            if ($i < $index*$step)
            {
                $currentSum += $value;
                $maxIndex = $i;
            }
            else
            {
                $resultValues[] = $currentSum;
                $resultKeys[] = GetCenterElement($minIndex, $maxIndex, $keys);

                $currentSum = $value;
                $minIndex = $i;
                ++$index;
            }
            ++$i;
        }

         if ($minIndex < $arrayCount- 1)
         {
             $resultValues[] = $currentSum;
             $resultKeys[] = GetCenterElement($minIndex, $maxIndex, $keys);
         }

         $Step = $step;
         $R = $r;

        return array_combine($resultKeys, $resultValues);
    }

    function CalculateR($arraySize)
    {
        $log2Size = log($arraySize, 2);
        if (floor($log2Size) == $log2Size)
            return intval($log2Size) - 1;
        else
            return intval(floor($log2Size));
    }


//class, that represents statistics
//operations on sample
class StatisticsCalculator
{
    private $Array;
    private $FrequencyTable;

    //statistics data
    public $mean;

    //count of all elements
    private $ArrayCount;

    public $median = null;
    public $standartDeviaton = null;
    public $sampleVariance = null;
    public $variance = null;
    public $variation = null;
    public $skew = null;
    public $kurtosis = null;    

    private $m2 = null;
    private $m3 = null;

    public function __construct($InputArray, $isContinuous=false)
    {
        //copy all values
        $this->Array = $InputArray;
        sort($this->Array);

        //count occurances of each number
        $this->FrequencyTable = array_count_values($this->Array);

        //sort keys in array
        ksort($this->FrequencyTable);

        if ($isContinuous)
            $this->FrequencyTable = EvalauteContinuousSampling($this->FrequencyTable);
        

        $this->ArrayCount = count($this->Array);
        $this->mean = array_sum($this->Array) / $this->ArrayCount;
    }

    public function nthMoment($a, $n)
    {
        $code = '$v += Power($w - '.$a.', '.$n.'); return $v;';
        $DiffNthPowerFunction = create_function('$v, $w', $code);
        return array_reduce($this->Array, $DiffNthPowerFunction, 0) / $this->ArrayCount;
    }

    private function GetSecondMoment()
    {
        if ($m2 === null)
            $m2 = $this->nthMoment($this->mean, 2);
        return $m2;
    }

    private function GetSampleVariance()
    {
        return $this->nthMoment($this->mean, 2);
    }

    private function GetThirdMoment()
    {
        if ($m3 === null)
            $m3 = $this->nthMoment($this->mean, 3);
        return $m3;
    }

    private function GetVariance()
    {
        $besselCorrection = $this->ArrayCount / ($this->ArrayCount - 1);
        return $this->SampleVariance() * $besselCorrection;
    }

    private function GetStandartDeviation()
    {
        return sqrt($this->Variance());
    }

    private function GetVariation()
    {
        return $this->StandartDeviation() / $this->mean;
    }

    private function GetSkew()
    {
        $m3 = $this->GetThirdMoment();
        $m2 = $this->GetSecondMoment();

        return $m3 / ($m2 * sqrt($m2));
    }

    private function GetKurtosis()
    {
        $m4 = $this->nthMoment($this->mean, 4);
        $m2 = $this->GetSecondMoment();

        return $m4 / ($m2 * $m2) - 3;
    }

    private function GetMedian()
    {
        $KIndex = $this->ArrayCount >> 1;
        if (($this->ArrayCount % 2) == 1)
            return $this->Array[$KIndex];
        else
            return ($this->Array[$KIndex] + $this->Array[$KIndex - 1]) / 2;
    }

    public function Quantile($alpha)
    {
        if ($alpha <= 0 Or $alpha >= 100)
            return null;

         $index = ($alpha * $this->ArrayCount) / 100;

        if (floor($index) == $index)
            return $this->Array[$index - 1];

        return null;
    }

    public function InnerQuantileWidth($alpha1, $alpha2)
    {
        $q1 = $this->Quantile($alpha1);
        $q2 = $this->Quantile($alpha2);

        if ($q1 != null And $q2 != null)
        {
            return $q2 - $q1;
        }

        return null;
    }

    public function Variance()
    {
        if ($this->variance === null)
            $this->variance = $this->GetVariance();

        return $this->variance;
    }

    public function SampleVariance()
    {
        if ($this->sampleVariance === null)
            $this->sampleVariance = $this->GetSampleVariance();

        return $this->sampleVariance;
    }

    public function Variation()
    {
        if ($this->variation === null)
            $this->variation = $this->GetVariation();

        return $this->variation;
    }
   
    public function StandartDeviation()
    {
         if ($this->standartDeviaton === null)
            $this->standartDeviaton = $this->GetStandartDeviation();

         return $this->standartDeviaton;
    }

    public function Skew()
    {
        if ($this->skew === null)
            $this->skew = $this->GetSkew();

        return $this->skew;
    }

    public function Kurtosis()
    {
        if ($this->kurtosis === null)
            $this->kurtosis = $this->GetKurtosis();

        return $this->kurtosis;
    }

    public function Mean()
    {
        return $this->mean;
    }

    public function Mode(&$outMode)
    {
       $outMode = max($this->FrequencyTable);
       return array_keys($this->FrequencyTable, $outMode);
    }

    public function Median()
    {
        if ($this->median === null)
            $this->median = $this->GetMedian();

        return $this->median;
    }

    public function ComputeStats()
    {
        $this->StandartDeviation();
        $this->SampleVariance();
        $this->Variance();
        $this->Variation();
        $this->Skew();
        $this->Kurtosis();
        $this->Median();
    }

    public function __destruct()
    {
        unset ($this->Array);
    }
}

?>