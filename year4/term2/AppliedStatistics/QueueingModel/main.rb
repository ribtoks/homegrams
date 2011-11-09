$i = 12
$sigma = 2

srand

def get_next_time
  -$sigma*Math.log(1.0 - rand)
end

def try_paste_element(queues, queues_sizes, element)
  added = false
  queues.each_index do |i|
    q = queues[i]
    if (q.size < queues_sizes[i])
      q.push element
      added = true
      break
    end
  end

  added
end

def get_element(queues, queue_index)
  unless queues[queue_index].empty?
    return queues[queue_index].shift
  else
    index = (queue_index + 1) % queues.size
    until index == queue_index
      unless queues[index].empty?
        return queues[index].shift
      else
        index = (index + 1) % queues.size
      end
    end
    # not found anything
  end
  nil
end

def provide_experiment(tasks_count)

  queues = [[], []]
  queues_max_count = [$i - 4, $i - 3]
  queue_index = 0
  task_time = $i
  
  queue_finish_time = 0
  curr_time = 0

  max_time = task_time * tasks_count

  m = 0
  v = 0

  while curr_time < max_time

    next_task_time = get_next_time

    # if current task will be finished
    # before next task is ready
    # that can take an element from queue
    if queue_finish_time < (curr_time + next_task_time)
      # we can take some element from queue
      element = get_element(queues, queue_index)


      # if we have elements in queues
      unless element.nil?
        # execute queue element
        #curr_time += task_time #element
        queue_finish_time += task_time #element
        curr_time += next_task_time
        
        # try to add to queue this element
        v += 1 unless try_paste_element(queues,
                                        queues_max_count,
                                        next_task_time)
        queue_index = (queue_index + 1) % queues.size
      else
        # execute new element 
        curr_time += next_task_time
        queue_finish_time += next_task_time + task_time
      end

      # queue will finish after this task
      # queue_finish_time += task_time
      # increment executed tasks counter
      m += 1
      
    else
      # try to push it to some queue
      v += 1 unless try_paste_element(queues,
                                      queues_max_count,
                                      next_task_time)
      queue_index = (queue_index + 1) % queues.size
      # and shift our time ahead
      curr_time += next_task_time
    end
    
  end  

  [m, v]
end

count = 100
tasks_count = 1000
m = v = 0
count.times do |i|
  arr = provide_experiment tasks_count
  m += arr[0]
  v += arr[1]
end

m /= count
v /= count

puts "m = #{m}"
puts "v = #{v}"
