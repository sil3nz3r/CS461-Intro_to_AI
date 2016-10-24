%------------------------------------------------------------------------------
% Name: Tu Vu
% Class: CS461 - Intro to A.I.
% Term: Fall 2016
%------------------------------------------------------------------------------

:- dynamic([
  agent_location/1,
  gold_location/1,
  pit_location/1,
  time/1,
  score/1,
  visited/1,
  visited_cells/1,
  potential_cells/1,
  world_size/1,
  wumpus_location/1
  ]).

%------------------------------------------------------------------------------
% Entry point of the program

start :-
    format("Initializing the Envorinment...~n", []),
    init,

    format("Let the game begin!~n", []),
    step([[0,0]]).

%------------------------------------------------------------------------------
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

%------------------------------------------------------------------------------
% Scheduling simulation:

step_pre(VisitedList) :-
  agent_location(AL),
  gold_location(GL),
  wumpus_location(WL),

  score(S),
  time(T),

  ( AL=GL -> format("WON!~n", []), format("Score: ~p,~n Time: ~p", [S,T])
  ; AL=WL -> format("Lost: Wumpus eats you!~n", []),
             format("Score: ~p,~n Time: ~p", [S,T])
  ; step(VisitedList)
  ).

step(VisitedList) :-
  /*create_percept_sentence(Perception),
  agent_location(AL),
  format("I'm in ~p, seeing: ~p~n", [AL, Perception]),

  update_KB(Perception),
  ask_KB(VisitedList, Action),
  format("I'm going to: ~p~n", [Action]),

  update_time,
  update_score,

  agent_location(Aloc),
  VL = [Aloc|VisitedList],
  standing,
  step_pre(VL)
  */
  create_percept_sentence(Perception),
  agent_location(AL),
  format("I'm in ~p, percepting: ~p~n", [AL, Perception]),

  ask_KB(VisitedList, Action),
  update_KB(Perception),
  format("Agent is going to: ~p~n", [Action]),
  update_agent_location(Action),

  update_time,
  update_score,

  agent_location(Aloc),
  VL = [Aloc|VisitedList],
  standing,
  step_pre(VL)
  .

%------------------------------------------------------------------------------
% Perception

test_perception :-
create_percept_sentence(Perception),
format("I feel ~p, ",[Perception]).

create_percept_sentence([Breeze, Stench, Glint, Bump, Scream, Has_gold, Dead, Escaped]) :-
  breezy(Breeze),
  smelly(Stench),
  glittering(Glint),
  is_by_wall(Bump),
  have_heard_wumpus(Scream),
  has_gold(Has_gold),
  is_agent_dead(Dead),
  has_agent_escaped(Escaped)
  .

%------------------------------------------------------------------------------
% Oracle

are_close([X, Y], [A, B]) :-
  % ; is OR , is AND
  (X is A , (Y >= B - 1 , Y =< B + 1))
  ;
  (Y is B, (X >= A + 1 , X =< A - 1))
  .

breezy(yes) :-
  agent_location(AL),
  pit_location(PL),
  are_close(AL, PL).
breezy(no).

smelly(yes) :-
  agent_location(AL),
  wumpus_location(WL),
  are_close(AL, WL).
smelly(no).

glittering(yes) :-
  agent_location(AL),
  gold_location(GL),
  are_close(AL, GL).
glittering(no).

is_by_wall(yes) :-
  agent_location([X, Y]),
  (X is 0)
  ;
  (Y is 0)
  .
is_by_wall(no).

have_heard_wumpus(yes) :-
  0 is 0.
is_by_wall(no).

has_gold(yes) :-
  0 is 0.
has_gold(no).

is_agent_dead(yes) :-
  0 is 0.
is_agent_dead(no).

has_agent_escaped(yes) :-
  0 is 0.
has_agent_escaped(no).

%------------------------------------------------------------------------------
% Knowledge Base:

update_KB([Breeze, Stench, Glint, Bump, Scream, Has_gold, Dead, Escaped]) :-
  add_pit_assumptions(Breeze),
  add_wumpus_assumptions(Stench),
  add_gold_assumptions(Glint)
  .

add_wumpus_assumptions(Stench) :-
  agent_location([X, Y]),
  (T1 is Y + 1 -> assume_wumpus(Stench, [X, T1])) ,
  (T2 is X + 1, assume_wumpus(Stench, [T2, Y])) ;
  (T4 is X - 1, X > 0 -> assume_wumpus(Stench, [X, T3])) ;
  (T4 is Y - 1, Y > 0  -> assume_wumpus(Stench, [X, T4]))
  .

add_pit_assumptions(Breezy) :-
  agent_location([X, Y]),
  (T1 is Y + 1 -> assume_pit(Breezy, [X, T1])) ,
  (T2 is X + 1, assume_pit(Breezy, [T2, Y])) ;
  (T4 is X - 1, X > 0 -> assume_pit(Breezy, [X, T3])) ;
  (T4 is Y - 1, Y > 0  -> assume_pit(Breezy, [X, T4]))
  .

add_gold_assumptions(Glint) :-
  agent_location([X, Y]),
  (T1 is Y + 1 -> assume_gold(Glint, [X, T1])) ,
  (T2 is X + 1, assume_gold(Glint, [T2, Y])) ;
  (T4 is X - 1, X > 0 -> assume_gold(Glint, [X, T3])) ;
  (T4 is Y - 1, Y > 0  -> assume_gold(Glint, [X, T4]))
  .

assume_wumpus(no, L) :-
  retractall( is_wumpus(_, L) ),
  assertz( is_wumpus(no, L) ),
  format("KB learn: There's no Wumpus at ~p ~n", [L]).

assume_wumpus(yes, L) :-
  retractall( is_wumpus(_, L) ),
  assertz( is_wumpus(yes, L) ),
  format("KB learn: There's probably a Wumpus at ~p ~n", [L]).

assume_pit(no, L) :-
  retractall( isPit(_, L) ),
  assertz( isPit(no, L) ),
  format("KB learn: There's no Pit at ~p ~n", [L]).

assume_pit(yes, L) :-
  retractall( isPit(_, L) ),
  assertz( isPit(yes, L) ),
  format("KB learn: There's probably a pit at ~p ~n", [L]).

assume_gold(no, L) :-
  retractall( isGold(_, L) ),
  assertz( isGold(no, L) ),
  format("KB learn: There's no gold at ~p ~n", [L]).

assume_gold(yes, L) :-
  retractall( isGold(_, L) ),
  assertz( isGold(yes, L) ),
  format("KB learn: There's probably a gold at ~p ~n", [L]).



ask_KB(Visited_list, Next_move) :-
  agent_location([X, Y]),
  world_size(S),
  (
  T1 is Y + 1, Y < S, should_agent_travel_here([X, Y], [X, T1]) ->
    Next_move = [X, T1];
  T2 is X + 1, X < S, should_agent_travel_here([X, Y], [T2, Y]) ->
    Next_move = [T2, Y];
  T4 is X - 1, X > 0, should_agent_travel_here([X, Y], [X, T3]) ->
    Next_move = [X, T3];
  T4 is Y - 1, Y > 0, should_agent_travel_here([X, Y], [T4, Y]) ->
    Next_move = [T4, Y]
  ).

should_agent_travel_here(Current_location, Locaiton) :-
  is_location_safe(Potential_move),
  are_close(Current_location, Potential_move),
  is_valid_location(Potential_move),
  is_not_member(Potential_move, Visited_list).

is_location_safe(Location) :-
  is_wumpus(no, Potential_move),
  isPit(no, Potential_move).

is_valid_location([X, Y]) :-
  world_size(WS),
  X >= 0, X < WS + 1,
  Y >= 0, Y < WS + 1.

%------------------------------------------------------------------------------
% Updating states

update_time :-
  time(T),
  NewTime is T+1,
  retractall( time(_) ),
  assertz( time(NewTime) ).

update_score :-
  agent_location(AL),
  gold_location(GL),
  wumpus_location(WL),
  update_score(AL, GL, WL).

update_score(P) :-
  score(S),
  NewScore is S+P,
  retractall( score(_) ),
  assertz( score(NewScore) ).

update_score(AL, AL, _) :-
  update_score(1000).

update_score(_,_,_) :-
  update_score(-1).

update_agent_location(NewAL) :-
  agent_location(AL),
  retractall( agent_location(_) ),
  assertz( agent_location(NewAL) ).

is_pit(no,  X).
is_pit(yes, X) :-
  pit_location(X).

%------------------------------------------------------------------------------
% Display standings

standing :-
  wumpus_location(WL),
  gold_location(GL),
  agent_location(AL),

  ( is_pit(yes, AL) -> format("Agent was fallen into a pit!~n", []),
    fail
  ; stnd(AL, GL, WL)
    %\+ pit_location(yes, Al),
  ).

stnd(AL, GL, WL) :-
  format("There's still something to do...~n", []).

stnd(AL, _, AL) :-
  format("YIKES! You're eaten by the wumpus!", []),
  fail.

stnd(AL, AL, _) :-
  format("AGENT FOUND THE GOLD!!", []),
  true.

%------------------------------------------------------------------------------
% Utils

is_not_member(_, []) :- !.

is_not_member([X, Y], [[X1, Y1] | Tail]) :-
  T is X + 1,
  V is Y + 1,
  T1 is X1 + 1,
  V1 is Y1 + 1,
  \+ (T is T1, V is V1),
  is_not_member([X, Y], Tail)
  .

read_list([]) :- !.

read_list([H, X, Y | T]) :-
  %write(H),
  atom_codes(X, XCodes),
  number_codes(XNumber, XCodes),
  atom_codes(Y, YCodes),
  number_codes(YNumber, YCodes),
  (H = 'GOLD' ->
    assertz(gold_location([XNumber,YNumber])),
    format('Insert Gold Location ~w', [[XNumber, YNumber]]),
    nl;
  H = 'PIT' ->
    assertz(pit_location([XNumber,YNumber])),
    format('Insert Pit Location ~w', [[XNumber, YNumber]]),
    nl;
  H = 'WUMPUS' ->
    assertz(wumpus_location([XNumber,YNumber])),
    format('Insert Wumpus Location ~w', [[XNumber, YNumber]]),
    nl
    ),
  read_list(T).

read_stream(Stream, []):-
  at_end_of_stream(Stream),
  !.

read_stream(Stream, [H|T]):-
% NOT end of stream
  \+  at_end_of_stream(Stream),
  read_each_word(Stream, H),
  read_stream(Stream, T).

read_each_word(InStream, Word):-
  get_code(InStream, Char),
  checkCharAndReadRest(Char, Chars, InStream),
  atom_codes(Word, Chars).

% 10 = new line
checkCharAndReadRest(10,[],_):-  !.
% 32 = blank
checkCharAndReadRest(32,[],_):-  !.
% -1 = end_of_stream
checkCharAndReadRest(-1,[],_):-  !.
% end_of_file
checkCharAndReadRest(end_of_file,[],_):-  !.

checkCharAndReadRest(Char,[Char|Chars],InStream):-
  get_code(InStream,NextChar),
  checkCharAndReadRest(NextChar,Chars,InStream).

atom_number(Atom, Number) :-
  atom_codes(Atom, Codes),
  number_codes(Number, Codes).
