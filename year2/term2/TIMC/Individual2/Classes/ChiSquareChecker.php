<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

require_once 'IntegralSolver.php';
require_once 'HTMLTable.php';
require_once 'WarningProvider.php';
/**
 * Description of ChiSquareChecker
 *
 * @author taras
 */
class ChiSquareChecker
{
    // number of elements in sampling
    private $n;

    // array of values
    private $m;

    // array of expected values
    private $nPi;

    // number of segments
    private $r;

    // degree of freedom
    private $df;

    // vector of input sampling
    private $x;

    //private $isContinuous;

    // object, that can solve integral equations
    private $integralSolver;

    // some critical values, that allow to use this method
    private $npiMinimal = 10;
    private $miMinimal = 5;

    private $debug = false;


    public function __construct($sampling, $samplingFrequencies,
        $probabilityFunc, $appreciatedArgsCount, $isDebug, $epsilon,
        $isContinuous, $segments)
    {
        $this->integralSolver = new IntegralSolver($epsilon);
        $this->n = array_sum($samplingFrequencies);
        $this->debug = $isDebug;

        $samplingSize = count($sampling);

        $this->x = $sampling;
        $this->m = $samplingFrequencies;

        $this->nPi = array();

        for ($i = 0; $i < $samplingSize; ++$i)
        {
            if ($isContinuous)
                $this->nPi[] = 
                    $this->n*$probabilityFunc($this->x[$i], 
                        $segments[$i]['a'], $segments[$i]['b']);
            else
                $this->nPi[] = $this->n*$probabilityFunc($this->x[$i]);
        }

        $tempSum = array_sum($this->nPi);
        if (abs($tempSum - $this->n) > 1)
            WarningProvider::Warn("My distribution function seems to be WRONG!!!
                    Real value - {$this->n} | My value - $tempSum");
        //else
        //    echo "<br /><br />Real value - {$this->n} | My value - $tempSum<br />";

        if ($this->debug)
        {
            echo "<br /><br />Frequencies and n*Pi:<br />";
            echo '<div style="margin-left:20px; margin-top:10px;">';
            HTMLTable::start(array("m[]", "nPi[]"));
            for ($i = 0, $count = count($this->m); $i < $count; ++$i)
                HTMLTable::printRow("", array($this->m[$i], $this->nPi[$i]));
            HTMLTable::end();
            echo "</div><br />";
        }

        $this->Preprocess_M_nPi();

        $this->r = count($this->m) - 1;
        $this->df = $this->r - $appreciatedArgsCount;

        if ($this->debug)
        {
            HTMLTable::start();
            HTMLTable::printRow("Sampling size", $samplingSize);
            HTMLTable::printRow("Degree of freedom", $this->df);
            HTMLTable::printRow("r", $this->r);
            HTMLTable::end();
            echo "<br />";
        }

        if ($this->df < 1)
            die('<font style="color:red; font-weight:bold;">Degree of freedom is less equal zero. Wrong sampling!!!
                    Can not calculate anything...</font>');
    }

    private function Preprocess_M_nPi()
    {
        if (min($this->m) >= $this->miMinimal
            && min($this->nPi) >= $this->npiMinimal)
            return;

        $mCount = count($this->m);

        $arr = array_combine($this->nPi, $this->m);
        arsort($arr);

        $tempM = array_values($arr);
        $tempNPi = array_keys($arr);

        // first find index in {m} array
        $mIndex = $mCount - 2;

        while ($tempM[$mIndex] < $this->miMinimal)
        {
            --$mIndex;
            if ($mIndex < 0)
                break;
        }
        ++$mIndex;

        if ($mIndex > 0)
            if (array_sum(array_slice($tempM, $mIndex)) < $this->miMinimal)
                --$mIndex;

        $newM = array_slice($tempM, 0, $mIndex);
        $newM[] = array_sum(array_slice($tempM, $mIndex));

        $newNPi = array_slice($tempNPi, 0, $mIndex);
        $newNPi[] = array_sum(array_slice($tempNPi, $mIndex));
        
        $arr = array_combine($newNPi, $newM);

        krsort($arr);

        $tempM = array_values($arr);
        $tempNPi = array_keys($arr);

        $npiIndex = count($tempNPi) - 1;
        while ($tempNPi[$npiIndex] < $this->npiMinimal)
        {
            --$npiIndex;
            if ($npiIndex < 0)
                break;
        }
        ++$npiIndex;

        if ($npiIndex > 0)
            if (array_sum(array_slice($tempNPi, $npiIndex)) < $this->npiMinimal)
                --$npiIndex;

        $newM = array_slice($tempM, 0, $npiIndex);
        $newM[] = array_sum(array_slice($tempM, $npiIndex));

        $newNPi = array_slice($tempNPi, 0, $npiIndex);
        $newNPi[] = array_sum(array_slice($tempNPi, $npiIndex));

        $this->m = $newM;
        $this->nPi = $newNPi;

        if ($this->debug)
        {
            echo "<br /><br />Preprocessed frequencies and n*Pi:<br />";
            echo '<div style="margin-left:20px; margin-top:10px;">';
            HTMLTable::start(array("m[]", "nPi[]"));
            for ($i = 0, $count = count($this->m); $i < $count; ++$i)
                HTMLTable::printRow("", array($this->m[$i], $this->nPi[$i]));
            HTMLTable::end();
            echo "</div><br /><br />";
        }
    }

    public function IsSamplingOk($alpha)
    {
        if ($this->debug)
        {
            HTMLTable::start();
            HTMLTable::printRow("Alpha", $alpha);
            
            $start = microtime(true);
        }

        $ChiSquare = 0;

        for ($i = 0; $i < $this->r + 1; ++$i)
        {
            $realValue = $this->nPi[$i];

            $numerator = $this->m[$i] - $realValue;
            $ChiSquare += $numerator*$numerator / $realValue;
        }

        // find critical value for this alpha and degree of freedom
        $ChiCritical = $this->integralSolver->SolveChiSquare($this->df, $alpha);

        $result = ($ChiSquare < $ChiCritical);
        
        if ($this->debug)
        {
            $end = microtime(true);
            HTMLTable::printRow("Empiric Value", $ChiSquare);
            HTMLTable::printRow("Critical Value", $ChiCritical);
            HTMLTable::printRow("Time elapsed (sec)", ($end - $start));

            echo '<tr><td colspan="2"></td></tr>';
            
            echo "<tr><th>TEST RESULT</th>";
            if ($result)
                $value = "Current hypothesis is correct.";
            else
                $value = "Current hypothesis is NOT correct";


            echo '<td style="color:white;" bgcolor="';
            if ($result) echo "green";
                    else echo "red";
            echo '">'.$value.'</td>';
            echo "</tr>";

            HTMLTable::end();
        }

        return $result;
    }
}
?>
