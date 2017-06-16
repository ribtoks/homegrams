include libmacro.inc
program Resheto_Eratosfena;
.data?
   n dw ?
   k dw ?
   a db 32536 dup(1)
   hell db 'Vvedit 4yslo vid 2 do 32536:',0
   druk db 'Drukuju prosti chysla vid 1 do ',0
   vsogo db 'Bulo nadrukovano ',0
   prost db 'prostyh 4ysel...',0
   
start
   wlinezln hell
   rword n
   mov k,0   

  
  mov si,2
  mov cx,n
cyklout: cmp a[si],1
  	push si
  	jne vyhid
	
       mov bx,si

        mov di,si
       mov ax,n
       cwd
       div di
       mov k,ax
  cyklin: cmp di,k
	jg vyhid
	mov ax,bx
	mul di
	mov si,ax
	mov a[si],0
	inc di
	jmp cyklin;
  vyhid:pop si
	inc si
   loop cyklout
      
      wlinez druk
      wwordln n
      wword 1	   
      mov ax,1
      mov si,2
      dec n
      mov cx,n
cykl: cmp a[si],1
      jne vyhid2
      wword si
       inc ax     
      vyhid2: inc si
 loop cykl
  
  linefeed
  
  wlinez vsogo 
 wword 
wlinez prost 
  
 
 linefeed 
 linefeed
 linefeed
 linefeed
rchar
return
end Resheto_Eratosfena 
	
 	




