:-dynamic(punto/2).

% Guardar punto en memoria.
guardar_punto(X,Y):- assert(punto(X,Y)).

% Eliminar punto de memoria.
eliminar_punto(X,Y):- retract(punto(X,Y)).

% Eliminar todos los puntos de memoria.
eliminar_todo:- retractall(punto(_,_)).

% Eliminar �ltimo de la lista.
eliminarultimo(L,L1):- reverse(L,[_|T]),reverse(T,L1).

% Funci�n auxiliar.
g(X,Y, Agregados,[[X2,Y]|R], N):-
	X2 is X+1,
	punto(X2,Y),
	L=[X2,Y],
	not(member(L, Agregados)),
	g(X2, Y, [L|Agregados],R, N2),
	N is N2+1.
g(X,Y, Agregados, [[X2,Y]|R], N):-
	X2 is X-1,
	punto(X2,Y),
	L=[X2,Y],
	not(member(L, Agregados)),
	g(X2, Y, [L|Agregados],R, N2),
	N is N2+1.

g(X,Y, Agregados,[[X,Y2]|R], N):-
	Y2 is Y+1,
	punto(X,Y2),
	L=[X,Y2],
	not(member(L, Agregados)),
	g(X, Y2, [L|Agregados], R, N2),
	N is N2+1.

g(X,Y, Agregados,[[X,Y2]|R], N):-
	Y2 is Y-1,
	punto(X,Y2),
	L=[X,Y2],
	not(member(L, Agregados)),
	g(X, Y2, [L|Agregados],R, N2),
	N is N2+1.

% Condici�n de parada.
g(_,_,_,[],0).

aplanar([],[_],[]).
aplanar([H1|T1],R,S):-
	aplanar(T1,R,S2),
	append(H1,S2,S3),
	S = S3.

%%%%%%%%%%%%%%%%%% Consultas %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Calcular el tama�o del grupo al que corresponde una casilla.
%
grupo_punto(X,Y,R,N):-
	findall(L,g(X,Y,[],L,_),R2),
	aplanar(R2,_,R3),
	sort(R3,R),
	length(R,N).

todoAux(_, _,[],[_],[]).
todoAux(X,Y,[[H1|[T1|_]]|T2],[R2|S3],S):-
	grupo_punto(X,Y,R2,_),
	todoAux(H1,T1,T2,S3,_),!,
	S=S3,!.



% Calcular el n�mero de grupos distintos y sus tama�os para toda la
% cuadr�cula.
%
todos_grupos(S):-
	 findall([X,Y],punto(X,Y),[[H1|[T1|_]]|T2]),
	 todoAux(H1,T1,T2,_,S2),
	 eliminarultimo(S2,S4),
	 sort(S4,S3),
	 S = S3,!.

conectado_con(X,Y):- X1 is X+1, punto(X1,Y),!.
conectado_con(X,Y):- X1 is X-1, punto(X1,Y),!.
conectado_con(X,Y):- Y1 is Y+1, punto(X,Y1),!.
conectado_con(X,Y):- Y1 is Y-1, punto(X,Y1),!.

conectado(X,Y):-conectado_con(X,Y).

noConectado(X,Y,[],1):-not(conectado(X,Y)).
noConectado(_,_,[],0).
noConectado(X,Y,[[H1|[T1|_]]|T2],N):-
	not(conectado(X,Y)),
	noConectado(H1,T1,T2,N2),
	N3 is N2+1,
	N = N3.

noConectado(_,_,[[H1|[T1|_]]|T2],N):-
	noConectado(H1,T1,T2,N).

puntosIndividuales(N):-
	findall([X,Y],punto(X,Y),[[H1|[T1|_]]|T2]),
	noConectado(H1,T1,T2,N2),
	N2 = N,!.
