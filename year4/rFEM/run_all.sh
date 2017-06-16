#!/bin/bash

rm results/* &> /dev/null
#rm result &> /dev/null

./main.rb

res_fn=$(echo results/*.dat)

#mv "$res_fn" "result"

echo "set pm3d
set dgrid3d 30,30
set hidden3d
splot \"$res_fn\" u 1:2:3 with lines
pause mouse close" | gnuplot
