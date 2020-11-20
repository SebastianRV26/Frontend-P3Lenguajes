:-dynamic(punto/2).

punto(10,1).
punto(10,2).

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

conectado_x(X,Y,X1):- X1 is X+1, punto(X1,Y),!.
conectado_x(X,Y,X1):- X1 is X-1, punto(X1,Y),!.
conectado_y(X,Y,Y1):- Y1 is Y+1, punto(X,Y1),!.
conectado_y(X,Y,Y1):- Y1 is Y-1, punto(X,Y1),!.

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

group(X,Y,L):-
	L2 = [X,Y],
	not(member(L2,L)),
	conectado_x(X,Y,X2),
	group(X2,Y,[L2|L]).
group(X,Y,L):-
	L2 = [X,Y],
	not(member(L2,L)),
	conectado_y(X,Y,Y2),
	group(X,Y2,[L2|L]).
group(X,Y, L):-conectado(X,Y),L=L.
