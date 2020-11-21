:-dynamic(punto/2).

punto(1,1).
punto(8,1).
punto(1,2).
punto(2,2).
punto(3,2).
punto(7,2).
punto(8,2).
punto(1,3).
punto(2,3).
punto(1,4).
punto(6,7).
punto(6,8).
punto(0,9).
punto(1,9).
punto(2,9).
punto(3,9).

%punto(10,1).
%punto(10,2).

%punto(4,2).
%punto(5,2).

%punto(5,4).

%punto(4,5).

%punto(8,4).
%punto(8,5).
%punto(8,6).
%punto(9,6).

punto(5,7).
punto(6,7).

%start :- dynamic(punto/2), % /2 parametros
%         consult('db.pl').

guardar_punto(X,Y):- assert(punto(X,Y)).
eliminar_punto(X,Y):- retract(punto(X,Y)).
eliminar_todo:- retractall(punto(_,_)).

% X posición X.
% Y posición Y.
% M: marca t o f.
%existePunto(X,Y,M).

% X posición X.
% Y posición Y.
% XS lista de posiciones X.
% XS lista de posiciones Y.
% esGrupo(X,Y,XS,YS).

% R anonimo.
% ?-agregar([[1,2],[3,4]], [5,6], R).
% R = [[5, 6], [1, 2], [3, 4]].
agregar(Lista, New, R):- R = [New|Lista].

conectado_con(X,Y):- X1 is X+1, punto(X1,Y),!.
conectado_con(X,Y):- X1 is X-1, punto(X1,Y),!.
conectado_con(X,Y):- Y1 is Y+1, punto(X,Y1),!.
conectado_con(X,Y):- Y1 is Y-1, punto(X,Y1),!.

conectado(X,Y):-conectado_con(X,Y).

%%%%%%%%%%%%%%%%%% Consultas %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
% Calcular el tamaño del grupo al que corresponde una casilla.
%


% Calcular el número de grupos distintos y sus tamaños para toda la
% cuadrícula.
%
% pos x, pos y, Lista y numeros
rp2(X,Y,L,N):-
	conectado(X,Y),
	L2=[X,Y],
	not(member(L2, L)),
	agregar(L, L2, R),
	N2 is N+1,
	X1 is X-1,
	rp2(X1, Y, R, N2).

rp(X,Y,L,N):-rp2(X,Y,L,N2), N = N2.

ruta2(Lugar, Lugar, _, []).
ruta2(Inicio, Fin, Visitados, [Inicio|Camino]):-
    conectado_con(Inicio, AlgunLugar),
    not(member(AlgunLugar, Visitados)),
    ruta2(AlgunLugar, Fin,[Inicio|Visitados], Camino).

ruta(Inicio, Fin, Camino):-
    Inicio\=Fin,
    ruta2(Inicio, Fin, [], Camino), write(Camino).

%g(X,Y, [], 0):-not(conectado_con(X,Y)).

g(X,Y, Agregados,[[X2,Y]|R], N):-
	X2 is X+1,
	punto(X2,Y),
	L=[X2,Y],
	not(member(L, Agregados)),
	g(X2, Y, [L|Agregados],R, N2),
	N is N2+1.
%g(X,Y, Agregados, N):-
%	X2 is X-1,
%	punto(X2,Y),
%	L=[X2,Y],
%	not(member(L, Agregados)),
%	agregar(Agregados, L, R), N2 is N+1,
%	g(X2, Y, R, N2),R=R.
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


%g(X,Y, Agregados, N):-
%	Y2 is Y-1,
%	punto(X,Y2),
%	L=[X,Y2],
%	not(member(L, Agregados)),
%	agregar(Agregados, L, R), N2 is N+1,
%	g(X, Y2, R, N2),R=R.
g(X,Y, Agregados,[[X,Y2]|R], N):-
	Y2 is Y-1,
	punto(X,Y2),
	L=[X,Y2],
	not(member(L, Agregados)),
	g(X, Y2, [L|Agregados],R, N2),
	N is N2+1.

g(_,_,_,[],0).

aplanar([],_,[]).
aplanar([H1|T1],R,S):-
	aplanar(T1,R,S2),
	append(H1,S2,S3),
	S = S3.

grupo_punto(X,Y,R,N):-
	findall(L,g(X,Y,[],L,_),R2),
	aplanar(R2,_,R3),
	sort(R3,R),
	length(R,N).

todoAux(_, _,[],_).
todoAux(X,Y,[[H1|[T1|_]]|T2],S2):-
	grupo_punto(X,Y,R2,_),
	write(R2),nl,
	todoAux(H1,T1,T2,[R2|S3]),!,S2 = S3.

todos_grupos(S):-
	 findall([X,Y],punto(X,Y),[[H1|[T1|_]]|T2]),
	 todoAux(H1,T1,T2,S2),
	 S = S2.
