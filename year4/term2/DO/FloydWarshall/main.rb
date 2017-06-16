#!/bin/ruby

require 'matrix'

# some improvements of default ruby Matrix class
class Matrix
  def []=(i, j, x)
    @rows[i][j] = x
  end

  def dump(firstLine = "")
    str = ""
    if firstLine != ""
      str << firstLine << "\n"
    end
    for i in 0...self.row_size
      space = ""
      for j in 0...self.column_size
        if self[i, j].to_f.finite?
          str << space << self[i,j].to_s
        else
          str << space << '.'
        end
        space = " "
      end
      str += "\n"
    end
    return str
  end
  
end

# ----------------------------------------

inf = 1.0/0.0

w = Matrix.rows([
                 [0,   5,   11,  inf, inf, inf, inf],
                 [inf,   0,   inf,  4, inf, 2, inf],
                 [inf,   inf,   0,  inf, 6, inf, inf],
                 [inf,   inf,   inf,  0, -6, 5, 25],
                 [inf,   inf,  inf,  inf, 0, inf, 22],
                 [inf,   inf,   3,  inf, 10, 0, inf],
                 [inf,   inf,   inf,  inf, inf, inf, 0]
                ])


size = w.row_size
rows = []

size.times do |i|
  rows << Array.new(size) {|j| (j == i) ? 0 : (j+1) }
end

s = Matrix.rows(rows)
# debug only
#puts d.dump

# ----------------------------------------
size.times do |k|
  size.times do |i|
    size.times do |j|
      
      if (w[i, k] + w[k, j]) < w[i, j]
        w[i, j] = w[i, k] + w[k, j]
        s[i, j] = k + 1
      end
      
    end
  end

  # debug print of matrices
  puts w.dump
  puts "\r\n"
  puts s.dump
  puts "------------------------------\r\n\r\n"
end

# ----------------------------------------
# --------- Find shortest way-------------
# ----------------------------------------

def find_way(m, l, matrix)
  stack = []
  stack << [m, l]
  result = []

  until stack.empty?
    a = stack.shift

    i, j = a

    k = matrix[i-1, j-1]

    if (k == j)
      result << a
      next
    end

    stack.unshift [k, j]
    stack.unshift [i, k]
  end

  result
end


# ----------------------------------------

[[1,5], [1,6], [2,7], [3, 7]].each do |arr|
  
  puts arr.inspect
  i, j = arr
  puts "way length: #{w[i - 1, j - 1]}"
  puts find_way(i, j, s).inspect
  puts "----------"
end

