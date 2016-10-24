% Enviuronment Initialization
init :-
    init_game,
    init_land_fig72,
    init_agent,
    init_wumpus.

init_game :-
    retractall( time(_) ),
    assertz( time(0) ),

    retractall( score(_) ),
    assertz( score(0) ),

    retractall( visited(_) ),
    assertz( visited(1) ),

    retractall( is_wumpus(_,_) ),
    retractall( isGold(_,_) ),

    retractall( visited_cells(_) ),
    assertz( visited_cells([]) ).

    retractall( potential_cells(_) ),
    assertz( potential_cells([]) ).

% To set the situation described in Russel-Norvig's book (2nd Ed.),
% according to Figure 7.2
init_land_fig72 :-
    retractall( world_size(_) ),
    assertz( world_size(5) ),

    retractall( gold_location(_) ),
    assertz( gold_location([3,2]) ),

    retractall( pit_location(_) ),
    assertz( pit_location([1,3]) ),
    assertz( pit_location([2,4]) ).

init_agent :-
    retractall( agent_location(_) ),
    assertz( agent_location([0,0]) ),

    visit([0,0]).

init_wumpus :-
    retractall( wumpus_location(_) ),
    assertz( wumpus_location([3,3]) ).

visit(Xs) :-
    visited_cells(Ys),
    retractall( visited_cells(_) ),
    assertz( visited_cells([Ys|Xs]) ).
