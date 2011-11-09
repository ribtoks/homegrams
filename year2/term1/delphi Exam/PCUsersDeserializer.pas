unit PCUsersDeserializer;

interface

uses
  Windows, Dialogs, SysUtils, Classes, UserClass;

type
  PCUserDeserializer = class(TComponent)
  private
    { Private declarations }
    OpenFileDialog : TOpenDialog;
    OpenFileName : string;

     procedure SetFileName(NewFileName:string);
  protected
    { Protected declarations }
  public
    { Public declarations }
    constructor Create(aOwner : TComponent); overload;
    constructor Create(aOwner : TComponent; NewFileName : string); overload;

    function Execute(var usrList : PCUserList) : boolean;
  published
    { Published declarations }
    property FileName : string read OpenFileName write SetFileName;
  end;

procedure Register;

implementation

procedure Register;
begin
  RegisterComponents('Simple Components', [PCUserDeserializer]);
end;

{ PCUserDeserializer }

constructor PCUserDeserializer.Create(aOwner: TComponent);
begin
   Inherited Create(aOwner);
   OpenFileDialog := TOpenDialog.Create(aOwner);
   OpenFileDialog.Filter := 'Text Files(*.txt)|*.txt|All Files(*.*)|*.*';
end;

constructor PCUserDeserializer.Create(aOwner: TComponent;
  NewFileName: string);
begin
   Inherited Create(aOwner);
   OpenFileDialog := TOpenDialog.Create(aOwner);
   OpenFileName := NewFileName;
   OpenFileDialog.Filter := 'Text Files(*.txt)|*.txt|All Files(*.*)|*.*';
end;

function PCUserDeserializer.Execute(var usrList: PCUserList) : boolean;
var
   f : text;
   i : integer;
   count : integer;
   name, pass : string;
   line : string;
   poss : integer;
begin
     if (OpenFileDialog.Execute) then
       begin
           OpenFileName := OpenFileDialog.Filename;
           assignFile(f, OpenFileName);
           reset(f);
           readln(f, count);
           usrList.Resize(count);
           for i := 0 to count - 1 do
            begin
              readln(f, line);
              //parsing the line
              poss := pos(' ', line);
              name := copy(line, 0, poss - 1);
              pass := copy(line, poss + 1, length(line) - poss);
              usrList.Users[i] := PCUser.Create(name, pass);
            end;
            close(f);
       end;
end;

procedure PCUserDeserializer.SetFileName(NewFileName: string);
begin
   OpenFileName := NewFileName;
end;

end.
 