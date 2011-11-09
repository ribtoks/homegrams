unit PCUsersSerializer;

interface

uses
  Windows, Dialogs, SysUtils, Classes, UserClass;

type
  PCUserSerializer = class(TComponent)
  private
    { Private declarations }
    SaveFileDialog : TSaveDialog;
    SaveFileName : string;

    procedure SetFileName(NewFileName:string);
  protected
    { Protected declarations }
  public
    { Public declarations }
    constructor Create(aOwner : TComponent); overload;
    constructor Create(aOwner : TComponent; NewFileName : string); overload;

    function Execute(usrList : PCUserList) : boolean;
  published
    { Published declarations }
    property FileName : string read SaveFileName write SetFileName;
  end;

procedure Register;

implementation

procedure Register;
begin
  RegisterComponents('Simple Components', [PCUserSerializer]);
end;

{ PCUsersSerializer }

constructor PCUserSerializer.Create(aOwner : TComponent);
begin
  Inherited Create(aOwner);
  SaveFileDialog := TSaveDialog.Create(aOwner);
  SaveFileDialog.Filter := 'Text Files(*.txt)|*.txt|All Files(*.*)|*.*';
end;

constructor PCUserSerializer.Create(aOwner : TComponent; NewFileName: string);
begin
  Inherited Create(aOwner);
  SaveFileName := NewFilename;
  SaveFileDialog := TSaveDialog.Create(aOwner);
  SaveFileDialog.Filter := 'Text Files(*.txt)|*.txt|All Files(*.*)|*.*';
end;

function PCUserSerializer.Execute(usrList : PCUserList): boolean;
var
   f : Text;
   i : integer;
begin
    if (SaveFileDialog.Execute) then
      begin
        SaveFileName := SaveFileDialog.FileName;
        if (pos('txt', SaveFileName) < 1) then
          SaveFileName := SaveFileName + '.txt';
        assignFile(f, SaveFileName);
        rewrite(f);
        writeln(f, usrList.Size);
        for i := 0 to usrList.Size - 1 do
          begin
            writeln(f, usrList.Users[i].ToString);
          end;
          close(f);
      end;
end;

procedure PCUserSerializer.SetFileName(NewFileName: string);
begin
   SaveFileName := NewFileName;
end;

end.
 