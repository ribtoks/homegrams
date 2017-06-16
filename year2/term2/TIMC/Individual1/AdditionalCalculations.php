<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
include_once 'Definings.php';
include_once STATISTICS_CALCULATOR_PATH;
include_once 'Classes/HTMLTable.php';

@session_start();

$statCalc = unserialize($_SESSION["StatCalc"]);

echo "<p style=\"margin-left: 20px;\">";

HTMLTable::start();

HTMLTable::printRow("",
         array("<strong>".$_REQUEST["quantile"]." (th/d) quantile</strong>",
         $statCalc->Quantile($_REQUEST["quantile"])));

 HTMLTable::printRow("",
         array("<strong>".$_REQUEST["momentN"]."(th/d) central moment</strong>",
          $statCalc->nthMoment($statCalc->mean, $_REQUEST["momentN"])));

 HTMLTable::printRow("",
         array("<strong>Difference between ".$_REQUEST["quantileLast"]." and
    ".$_REQUEST["quantileFirst"]." quantiles</strong>",
         $statCalc->InnerQuantileWidth($_REQUEST["quantileFirst"], $_REQUEST["quantileLast"])));

 HTMLTable::end();

 echo "</p>";

 echo "<br />";
 echo "<br /><strong>Additional Calculations</strong>";
 include 'AdditionalForm.html';
?>
