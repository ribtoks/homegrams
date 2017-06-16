<!--
To change this template, choose Tools | Templates
and open the template in the editor.
-->
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN">

<?php
    include_once 'Classes/HTMLTable.php';
    include_once 'Classes/PHPImage.php';
    include_once 'Definings.php';

    include_once STATISTICS_CALCULATOR_PATH;

    //prints array in table:
    //first row are keys
    //second row are values
    function PrintHashMapInTable($frequencyTable)
    {
        HTMLTable::start();
        HTMLTable::printRow("Value", array_keys($frequencyTable));
        HTMLTable::printRow("Quantity", array_values($frequencyTable));
        HTMLTable::end();
    }   

     @session_start();
?>

<html>
    <head>
        <meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
        <title>Statistics calculations</title>
    </head>
    <body style="background: url(Images/calc.png) repeat-y;">
        <div style="margin-left:70px;">
<?php

    //get our array
    if (isset ($_REQUEST["rand"]))
    {
          srand(time());
          //generate random numbers
           for ($i = 0; $i < $_REQUEST["numcount"]; $i++)
               $ints[] = rand($_REQUEST["minvalue"], $_REQUEST["maxvalue"]);
    }
    else
            // input array
            $ints = explode($_REQUEST['separator'], $_REQUEST["textBox1"]);

            sort($ints);

            if (isset($_REQUEST['IsContinuous']))
            {
                $isContinuous = true;
                $continuousString = "True";
            }
            else
            {
                $isContinuous = false;
                $continuousString = "False";
            }


        echo "<strong>&nbsp;<u>Sampling information:</u></strong><br /><br />";
        echo "<i>Count:</i> ".count($ints)."<br /><br />";

        echo "<i>Minimum value:</i> ".min($ints)."<br /><br />";
        echo "<i>Maximum value:</i> ".max($ints)."<br /><br />";

        echo "<i>Sorted input values:</i><br />";
        echo "<p style=\"margin-left:30px\">";
        echo implode(", ", $ints);
        echo "</p>";

        echo "<i>Is continuous: </i>$continuousString<br /><br />";

        //count occurances of each number
        $FrequencyTable = array_count_values($ints);

        //sort keys in array
        ksort($FrequencyTable);

        echo "<br />";
        echo "&nbsp;<strong>Frequency table:</strong><br /><br />";
        //print table of frequency
        PrintHashMapInTable($FrequencyTable);
        echo "<br /><br />";

        if ($isContinuous)
        {
            $step = 0;
            $r = 0;
            $continuousFreqTable = EvalauteContinuousSampling($FrequencyTable, $step, $r);

            echo "&nbsp;<i>Frequency table count:</i> ".count($FrequencyTable)."<br /><br />";;
            echo "&nbsp;<i>Step:</i> $step<br /><br />";
            echo "&nbsp;<i>Parts number (r + 1):</i> $r + 1<br /><br />";

            echo "&nbsp;<strong>Continuous Sample frequency table:</strong><br /><br />";
            PrintHashMapInTable($continuousFreqTable);
            echo "<br /><br />";
        }

        echo "<hr width=\"50%\" align=\"left\" /><br />";

        $_SESSION["FrTable"] = $FrequencyTable;

        if (!$isContinuous)
        {
            PHPImage::image("Frequency diagram", FREQUENCY_DIAGRAM_PATH);
            echo "<br /><br />";
            PHPImage::image("Frequency polygon", FREQUENCY_POLYGON_PATH);
            echo "<br /><br />";
        }
        else
        {
            $_SESSION["ContFreqTable"] = $continuousFreqTable;
            PHPImage::image("Histogram", FREQUENCY_HISTOGRAM_PATH);
            echo "<br /><br />";
        }

        PHPImage::image("Graphic", GRAPHIC_PATH);
        echo "<br /><br />";

        echo "<hr width=\"50%\" align=\"left\" /><br />";
//-------------------------------------------------------------------------------------------------------------------------
        echo "<strong>Other statistics information:</strong><br />";

        $statCalc = new StatisticsCalculator($ints);
        $statCalc->ComputeStats();


        echo "<p style=\"margin-left: 20px;\">";
        HTMLTable::start();

        //printing results of calculations
        $statisticsData = get_object_vars($statCalc);

        foreach ($statisticsData as $propertyName => $propertyValue)
            HTMLTable::printRow("", array("<strong>".ucfirst($propertyName)."</strong>", $propertyValue));

        $mode = null;
        $modesKeys = $statCalc->Mode($mode);
        $modeString = $mode." - (".implode(", ", $modesKeys).")";
        HTMLTable::printRow("", array("<strong>Mode</strong>", $modeString));
        HTMLTable::end();

        echo "<br /><hr width=\"50%\" align=\"left\" />";

         $_SESSION["StatCalc"] = serialize($statCalc);

         echo "<iframe src=\"AdditionalForm.html\" frameborder=\"0\" width=\"100%\" height=\"500px\" />";
       ?>
        </div>
    </body>
</html>
