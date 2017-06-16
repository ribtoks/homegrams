def read_all(filename)
  # time interval is a day
  time_delta = 1.0
  start_time = 1.0

  values = open(filename).readlines.map {|x| x.to_f}
  
  times = Array.new(values.size) {|i| start_time + i * time_delta}

  [times, values]
end
