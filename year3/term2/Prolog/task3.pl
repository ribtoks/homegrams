ancestor(john, mike).
ancestor(mary, mike).

ancestor(jack, john).
ancestor(elizabeth, john).

ancestor(charly, mary).
ancestor(pamela, mary).

ancestor(greg, jack).
ancestor(jessica, jack).

ancestor(jacob, elizabeth).
ancestor(jane, elizabeth).

ancestor(nelson, charly).
ancestor(amanda, charly).

ancestor(bart, pamela).
ancestor(maggy, pamela).


male(mike).
male(john).
male(jack).
male(greg).
male(jacob).
male(charly).
male(nelson).
male(bart).

female(mary).
female(elizabeth).
female(jessica).
female(jane).
female(amanda).
female(pamela).
female(maggy).

different(X, Y):-X=Y,!,fail;true.

equal(X, Y) :- X=Y,!,true;fail.

sister(X, Y) :- female(X), ancestor(Z, X), ancestor(Z, Y), different(X, Y).

brother(X, Y) :- male(X), ancestor(Z, X), ancestor(Z, Y), different(X, Y).

father(X, Y) :- male(X), ancestor(X, Y).
mother(X, Y) :- female(X), ancestor(X, Y).

child(X, Y) :- ancestor(Y, X).

ancestors(X, Y) :- ancestor(X, Y).
ancestors(X, Z) :- ancestor(X, Y), ancestors(Y, Z).

grandfather(X, Z) :- male(X), ancestor(X, Y), ancestor(Y, Z).
grandmother(X, Z) :- female(X), ancestor(X, Y), ancestor(Y, Z).