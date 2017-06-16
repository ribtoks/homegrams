#!/usr/bin/env bash

pushd $(dirname $0) &>/dev/null || exit 1
PYTHONPATH=$PWD/..:$PYTHONPATH ./tests.py
popd &>/dev/null
