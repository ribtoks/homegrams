#!/usr/bin/env bash

pushd $(dirname $0) &>/dev/null || exit 1
PYTHONPATH=$PWD/..:$PYTHONPATH ./test-triangulation.py
popd &>/dev/null
