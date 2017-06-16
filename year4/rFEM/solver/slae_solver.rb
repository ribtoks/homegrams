require 'gsl'
require 'matrix'

class SLAE_Solver
  class << self

    def solve_linear_system(a_matr, b_vect)
      m = stupid_create(a_matr)
      b = GSL::Vector[b_vect]

      x = GSL::Linalg::LU.solve(m, b)

      x
    end

    private

    def stupid_create(m)
      n = m.row_size
      a = GSL::Matrix.alloc(n, n)

      n.times do |i|
        n.times do |j|
          a[i, j] = m[i, j]
        end
      end

      a
    end
    
  end
end
