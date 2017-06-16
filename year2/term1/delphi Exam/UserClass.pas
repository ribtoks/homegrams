unit UserClass;

interface
uses SysUtils;
type
     PCUser = class(TObject)
        private
           userName : string;
           userPassword : string;
        public
           constructor Create; overload;
           constructor Create(Name : string; Password : string); overload;
           constructor Create(usr : PCUser); overload;
           function ToString : string;
     end;

     PCUserList = class(TObject)
        private
           PCUsers : array of PCUser;
           listSize : integer;
           procedure SetPCUser(index : integer; value : PCUser);
           function GetPCUser(index : integer) : PCUser;
        public
           constructor Create(number : integer);
           procedure Resize(newSize : integer);

           property Users[index : integer] : PCUser read GetPCUser write SetPCUser;
           property Size : integer read listSize;
     end;

     PCUserListBoundsException = class(Exception)

     end;

implementation

{ PCUser }

constructor PCUser.Create;
begin
  Inherited;
end;

constructor PCUser.Create(Name, Password: string);
begin
   Inherited Create;
   userName := Name;
   userPassword := Password;
end;

constructor PCUser.Create(usr: PCUser);
begin
   userName := usr.userName;
   userPassword := usr.userPassword;
end;

function PCUser.ToString: string;
var res : string;
begin
   res := userName;
   res := res + ' ' + userPassword;
   result := res;
end;

{ PCUserList }

constructor PCUserList.Create(number: integer);
begin
  Inherited Create;
  SetLength(PCUsers, number);
  listSize := number;
end;

function PCUserList.GetPCUser(index: integer): PCUser;
begin
  if (index >= 0) and (index < listSize) then
     result := PCUsers[index]
  else
      raise PCUserListBoundsException.Create('Index was out of bounds!');
end;

procedure PCUserList.Resize(newSize: integer);
var newB : array of PCUser;
    i : integer;
begin
   SetLength(newB, listSize);
   for  i := 0 to listSize - 1 do
      newB[i] := PCUsers[i];
   SetLength(PCUsers, 0);
   SetLength(PCUsers, newSize);
   listSize := newSize;
   for i := 0 to listSize - 1 do
        PCUsers[i] := PCUser.Create(newB[i]);
   SetLength(newB, 0);
end;

procedure PCUserList.SetPCUser(index: integer; value: PCUser);
begin
  if (index >= 0) and (index < listSize) then
      PCUsers[index] := value
      else
          raise PCUserListBoundsException.Create('Index was out of bounds!');
end;

end.
