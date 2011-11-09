require 'matrix'

def point_ok(x)
  x1 = x[0]
  x2 = x[1]
  
  return false unless x1 >= 0.0
  return false unless x2 >= 0.0
  
  return false unless (2.0*x1 - x2) <= 6.0
  return false unless (x1 + 2.0*x2) <= 10.0

  true
end

def my_func(x)
  x1 = x[0]
  x2 = x[1]

  2.0*x1*x2 - x1*x1 - x2*x2
end

def get_rand_ksi()
  r = 2.0*Math::PI * rand
  arr = Array.new(2)
  # stupid hardcode one more time..
  arr[0] = Math.cos(r)
  arr[1] = Math.sin(r)
  Vector.elements(arr)
end

def get_ksi_arr(sz, func_con, x, a)
  # count = 0
  # arr = []
  # while count < sz
  #   ksi = get_rand_ksi
    
  #   if func_con.call(x + ksi*a)
  #     arr << ksi
  #     count += 1
  #   end
  # end
  # arr
  Array.new(sz){|i| get_rand_ksi}
end

def get_min_ksi(func, x, ksi_arr, a)
  ksi_arr.min_by {|ksi| func.call(x + ksi * a)}
end

def gen_delta_grad(func, x, ksi_star, ksi_arr, a)
  sum = 0
  f_x = func.call(x)

  ksi_arr.each do |ksi|
    sum += func.call(x + ksi*a) - f_x
  end

  sum = func.call(x + ksi_star*a) - f_x

  #ksi_star * (2*sum / a)
  ksi_star * (2*sum/(a*ksi_arr.size))
end

def func_min(x0, func, con_func)

  max_attempts = 100
  m = 10
  dim = 2
  
  w = Vector.elements(Array.new(dim, 0))
  
  k = 0.7
  #ksi = get_rand_ksi dim
  a = 0.5
  alpha = a
  
  x_curr = x0
  
  iterations = 0

  regenerate = true

  ##############################
  ksi_arr, ksi, delta_grad = 0
  counter = 0
  
  # cache curr func value
  f_curr = func.call x_curr
  
  loop do

    if regenerate
      
      counter += 1
      break unless counter < max_attempts
      
      # generate array of random directions
      ksi_arr = get_ksi_arr(m, con_func, x_curr, a)
      ksi = get_min_ksi(func, x_curr, ksi_arr, a)
      
      # get curr delta gradient
      delta_grad = gen_delta_grad(func, x_curr, ksi, ksi_arr, a)

      w = Vector.elements(Array.new(dim, 0))

      regenerate = false      
    end

    x_next = x_curr - delta_grad * alpha

    unless con_func.call(x_next)
      regenerate = true
      next
    end

    f_next = func.call x_next

    unless f_next < f_curr
      regenerate = true
      next
    end
          
    delta_f = f_next - f_curr
    delta_x = x_next - x_curr
    w = w*k - delta_x * alpha * delta_f

    ksi_arr.map! {|x| x + w}
    ksi += w
    # get curr delta gradient
    delta_grad = gen_delta_grad(func, x_curr, ksi, ksi_arr, a)
    
    counter = 0
    x_curr = x_next
    f_curr = f_next
    
    iterations += 1
    
    
  end

  return [x_curr, iterations]
    
end
  
srand

x0 = Vector[0.3, 2.5]
#puts point_ok(x0)

f = lambda{|x| -my_func(x)}
check_point = lambda{|x| point_ok(x)}

x_min, iterations = func_min x0, f, check_point


puts x_min
puts my_func(x_min)
#puts iterations
