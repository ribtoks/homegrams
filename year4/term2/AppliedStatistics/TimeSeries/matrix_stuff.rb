require 'matrix'

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
        str << space << self[i,j].to_s
        space = " "
      end
      str += "\n"
    end
    return str
  end
  
end
