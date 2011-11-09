require 'array_stuff.rb'
require 'matrix'
require 'matrix_stuff.rb'
require 'read_values.rb'
require 'draw_values.rb'

def smooth_array(arr, n)

  new_arr = Array.new(arr)
  
  return new_arr if n <= 0
  
  offset = n / 2
  
  start = offset
  finish = arr.size - offset
  
  (start...finish).each do |i|
    
    s = i - offset
    f = i + offset
    
    new_arr[i] = arr[s..f].mean
  end

  new_arr
end

def smooth_with_wma(arr, wma_func, n)
  new_arr = Array.new(arr)
  
  return new_arr if n <= 0

  start = n
  finish = -n

  (start...finish).each do |i|
    new_arr[i] = wma_func.call(arr, i)
  end
  new_arr
end

def find_k(arr)
  prev_diffs = arr.diffs
  curr_diffs = arr.diffs.diffs

  puts "\n"
  puts prev_diffs.max_abs
  k = 1  
  
  while curr_diffs.max_abs < 1.0 #curr_diffs.max_abs < prev_diffs.max_abs
    prev_diffs = Array.new(curr_diffs)
    curr_diffs = curr_diffs.diffs

    puts prev_diffs.max_abs
    k += 1

    break if curr_diffs.compact.size == 1
  end

  puts "----------"
  puts curr_diffs.max_abs
  puts prev_diffs.max_abs
  puts "----------"

  k
end

# weightted moving average

def get_wma5(y, pos)
  coefs = [-3, 12, 17, 12, -3]
  coefs.map!{|c| c / 35.0}

  a = 2
  y_vect = y[(pos - a)..(pos+a)]

  y_vect.scalar_product(coefs)
end

def get_wma7(y, pos)
  coefs = [-2, 3, 6, 7, 6, 3, -2]
  coefs.map!{|c| c / 21.0}

  a = 3
  y_vect = y[(pos - a)..(pos+a)]

  y_vect.scalar_product(coefs)
end


##############################

# read values from file
time_values, values = read_all 'my_currencies'

# smoothing region
smooth_count = 2
n = 3
all_arrays = [values]
# smooth array some number of times
temp_arr = values
smooth_count.times do |i|
  temp_arr = smooth_array temp_arr, n
  # smoothed_arrays << temp_arr
end
values = temp_arr
all_arrays << temp_arr

puts values.inspect

# find power of polynomial
k = find_k values

k = 6

temp_arr = values

values.size.times do |i|  
  puts "#{i} - #{temp_arr.max_abs}"
  temp_arr = temp_arr.diffs
end

puts "\nk == #{k}\n\n"

##############################

n = values.size

y = Array.new(n)
matr_rows = []

n.times do |i|
  y[i] = values[i]

  row = Array.new(k) { |j| time_values[i]**j }
  matr_rows << row
end

matr_X = Matrix.rows(matr_rows)
matr_Xt = matr_X.transpose
y_column = Matrix.column_vector(y)

##############################

coefs = (matr_Xt * matr_X).inverse * (matr_Xt * y_column)
coefs = coefs.column(0)

puts "coefs = #{coefs.inspect}"
# now build polynom as a string
polynom_arr = []

coefs.size.times do |i|
  #j = coefs.size - i - 1
  str = "(#{coefs[i]}"
  str << "*x**#{i}"  unless i.zero?
  str << ")"
  
  polynom_arr << str
end

polynom = polynom_arr.join("+")


##############################

m5_data = smooth_with_wma(values, method(:get_wma5), 2).map{|x|x + 0.9}
m7_data = smooth_with_wma(values, method(:get_wma7), 3).map{|x| x + 1}

##############################

draw_datasets time_values, values, polynom, m5_data, m7_data
