<?php
/* 
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 * Description of HTMLTable
 *
 * @author taras
 */
class HTMLTable
{
    //put your code here
    static function start($header=false, $cellspacing=5, $cellpaddiing=10)
    {
        print ("<table border=\"1\" cellspacing=\"$cellspacing\" cellpadding=\"$cellpaddiing\">");

        if (is_array($header))
        {
            print("<tr align=\"center\">\n");

            foreach($header as $h)
                print("<th>$h</th>\n");

            print("</tr>\n");
        }
    }

    static function end()
    {
        print("</table>\n\n");
    }

    static function printRow($label, $field)
    {
        print("<tr align=\"center\">\n");

        //description
        if ($label !== "")
            print("<th>$label</th>\n");

        if (!is_array($field))
            $field = array($field);

        foreach($field as $key => $value)
        {
            print("<td>\n");

            if ($value === "")
                print("&nbsp;");
            else
                print($value);

            print("</td>\n");
        }

        print ("</tr>\n");
    }

    static function printSet($set)
    {
        foreach($set as $field)
        {
            if (isset ($field['label']))
            {
                $label = $field['label'];
                unset($field['label']);
            }
            else
                $label = "";
            HTMLTable::printRow($label, $field);
        }
    }
}
?>
