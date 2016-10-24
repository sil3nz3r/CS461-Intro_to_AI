% Enviuronment Initialization
init :-
  init_geography,
  init_game,
  init_agent.

init_game :-
  retractall( time(_) ),
  assertz( time(0) ),

  retractall( score(_) ),
  assertz( score(0) ),

  retractall( visited(_) ),
  assertz( visited(1) ),

  retractall( isWumpus(_,_) ),
  retractall( isGold(_,_) ),

  retractall( visited_cells(_) ),
  assertz( visited_cells([]) ).

init_agent :-
  retractall( agent_location(_) ),
  assertz( agent_location([0,0]) ),

  visit([0,0]).

visit(Xs) :-
  visited_cells(Ys),
  retractall( visited_cells(_) ),
  assertz( visited_cells([Ys|Xs]) ).

init_geography :-
  retractall( world_size(_) ),
  retractall( gold_location(_) ),
  retractall( pit_location(_) ),
  retractall( wumpus_location(_) ),

  %Set world dimension
  assertz(world_size(5)),

  %Get the rest of the world details in an external file
  open('details.txt', read, Stream),
  read_stream(Stream, Words),
  close(Stream),
  read_list(Words).
