unit Unit1;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, StdCtrls, UserClass, XPMan, ComCtrls, ToolWin, Menus, ImgList,
  ActnList, PCUsersSerializer, PCUsersDeserializer;

type
  TForm1 = class(TForm)
    Button1: TButton;
    MainMenu1: TMainMenu;
    File1: TMenuItem;
    Operations1: TMenuItem;
    Help1: TMenuItem;
    ToolBar1: TToolBar;
    StatusBar1: TStatusBar;
    ToolButton1: TToolButton;
    XPManifest1: TXPManifest;
    Exit1: TMenuItem;
    Serialize1: TMenuItem;
    Deserialize1: TMenuItem;
    Nothing1: TMenuItem;
    Notworkingreference1: TMenuItem;
    ToolButton2: TToolButton;
    ToolButton3: TToolButton;
    ToolButton4: TToolButton;
    ActionList1: TActionList;
    Serialize: TAction;
    ImageList1: TImageList;
    Deserialize: TAction;
    CloseAction: TAction;
    Memo1: TMemo;
    PCUserSerializer1: PCUserSerializer;
    PCUserDeserializer1: PCUserDeserializer;
    procedure Button1Click(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure CloseActionExecute(Sender: TObject);
    procedure SerializeExecute(Sender: TObject);
    procedure DeserializeExecute(Sender: TObject);

  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  Form1: TForm1;
  usrList : PCUserList; 
implementation

{$R *.dfm}

procedure TForm1.Button1Click(Sender: TObject);
var i : integer;
  mess : string;
begin
  try
    for i := 0 to usrList.Size - 1 do
        Memo1.Lines.Add(usrList.Users[i].ToString);
  except
    on E:PCUserListBoundsException do
      begin
         mess := E.Message;
         Application.MessageBox(PAnsiChar(mess), 'Error');
      end;
  end;
end;

procedure TForm1.FormCreate(Sender: TObject);
begin
  usrList := PCUserList.Create(5);
  usrList.Users[0] := PCUser.Create('taras', 'password0');
  usrList.Users[1] := PCUser.Create('atras', 'password1');
  usrList.Users[2] := PCUser.Create('traas', 'password2');
  usrList.Users[3] := PCUser.Create('taars', 'password3');
  usrList.Users[4] := PCUser.Create('tarsa', 'password4');

  PCUserSerializer1 := PCUserSerializer.Create(self);
  PCUserDeserializer1 := PCUserDeserializer.Create(self);
end;



procedure TForm1.CloseActionExecute(Sender: TObject);
begin
    self.Close;
end;

procedure TForm1.SerializeExecute(Sender: TObject);
begin
  if (PCUserSerializer1.Execute(usrList)) then
    begin
      Memo1.Lines.Add('CurrentList was successfuly serialized.');
    end
    else
       Memo1.Lines.Add('Were problems in serializing.');
end;

procedure TForm1.DeserializeExecute(Sender: TObject);
var
  i : integer;
begin
   if (PCUserDeserializer1.Execute(usrList)) then
    begin
      Memo1.Lines.Add('CurrentList was successfuly deserialized.');
       for i := 0 to usrList.Size - 1 do
         Memo1.Lines.Add('     ' + usrList.Users[i].ToString);
        end
    else
       Memo1.Lines.Add('Were problems in deserializing.');
    Memo1.Lines.Add('- - - - - - - - - - - - - - - - ');
end;

end.
 