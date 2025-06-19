Présentation:
Notre projet Unity simule des interactions culinaires à la première personne. Le joueur peut ramasser des ustensiles (cuillère, spatule), des aliments (œufs, beurre), les déposer dans des contenants ou sur des surfaces de cuisson, et interagir avec des systèmes de cuisson pour produire des plats simples comme l’omelette, les œufs au plat ou les œufs brouillés.

Contrôles clavier
Touche		Action							Condition
E		Ramasser ou déposer un objet				Regarder l'aliment ou l'ustensile
E		Ouvrir et fermer les portes (ou le four)		Regarder la cible
B		Mélanger avec la spatule 				Avoir la spatule et regarder l'œuf pas cuit
C		Déclenche la cuisson sur la poêle			Placer l'œuf au dessus du beurre fondu (avec E)
M		Mélange un œuf dans le bol avec une cuillère		Avoir la cuillère et regarder l'œuf posé sur le bol

Description des scripts:

RamasseUstensile.cs
Gère le ramassage de la spatule.

Le joueur peut tenir un seul ustensile à la fois.

Si la touche B est pressée avec une spatule en main et qu’un œuf pas encore cuit est visé, le mode œufs brouillés est activé.

RamasseCuillere.cs
Spécialisation simple pour le ramassage d’une cuillère.

Permet de la tenir et de la relâcher en alternant sur la touche E.

Les collisions et la physique sont gérées dynamiquement.

RamasseAliment.cs
Permet de ramasser des aliments ou le bol (seulement si ce dernier contient l'œuf mélangé).

Permet de déposer les objets tenus sur des points désignés (poêle ou bol).

Si un bol rempli est déposé sur la poêle, une omelette est instanciée.

Gère aussi l’instanciation dynamique d’aliments à partir de prefabs lors du ramassage.

CuissonOeufAuPlat.cs
Gère la cuisson des œufs (au plat ou brouillés).

Nécessite que du beurre fonde (à l'aide d'un timer) sur la poêle pour activer la cuisson.

Un œuf peut être cassé en pressant la touche C.

Si le mode "brouillé" est activé pendant la cuisson, un prefab d’œufs brouillés est instancié à la fin au lieu d'un œuf au plat classique.

BowlMix.cs
Si le joueur tient une cuillère et presse la touche M en visant le bol contenant un œuf, l’œuf est remplacé par EggFond (le mélange d'œuf).

Cette étape est nécessaire pour rendre le bol "valide" pour pouvoir le prendre en main par la suite.

RamasseAssiette.cs
Permet au joueur de ramasser une assiette instanciée depuis un prefab.

Si une assiette est tenue et qu’un œuf au plat est visé, celui-ci est ajouté dans l’assiette.

Sinon, l’assiette peut être déposée sur un meuble. (ne fonctionne plus correctement)

Liste des tags utilisés
Les tags sont essentiels pour que les scripts reconnaissent et interagissent avec les objets. Voici les tags attendus :

Spoon : cuillère

Spatula : spatule

Food : aliments divers

EggBox : boîte d'œufs

Butter : beurre

EggHand : œuf tenu à la main

EggFond : contenu d’œuf dans un bol

Bowl : bol

Poele : poêle

Plate : assiette

FriedEgg : œuf au plat

Meuble : meuble pouvant recevoir une assiette

Prefabs utilisés
Le fonctionnement repose sur l’instanciation dynamique de prefabs. Voici les principaux :

EggHand (œuf ramassé)

EggFond (œuf mélangé)

OeufPasCuit

OeufCuit

OeufBrouilles

FondJaune (beurre fondu)

Butter

Bowl

Spoon

Spatula

Plate

Tous les prefabs doivent inclure :

Un Box Collider (la hitbox)

Un Rigidbody (pour qu'il n'aie pas de comportement involontaire)

Le tag correspondant