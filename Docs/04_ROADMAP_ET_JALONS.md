# 04 - Roadmap et Jalons de Production

## Vue d'Ensemble des Jalons (Milestones)

L'objectif initial est la production d'une **Vertical Slice** (tranche verticale jouable de 10 a 15 minutes) permettant de valider l'interet du gameplay, le feeling des combats et la reception du lore.

---

## Jalon 1 : Prototype Fondations (Semaines 1 a 3)
* Objectif : Valider les sensations physiques du joueur dans une arène sans graphismes finaux (Greybox).
* Livrables :
  * Player Controller fluide et reactif.
  * Systeme d'esquive / roulade avec invulnerabilite (i-frames).
  * Barre de Stamina avec regles de consommation et de regeneration.
  * Combo de base a l'epee avec retours sensoriels (Hit Stop, Screen Shake).
  * Un mannequin d'entrainement ou ennemi passif reagissant aux impacts.

---

## Jalon 2 : Premier Combat Complet & IA (Semaines 4 a 6)
* Objectif : Mettre en place la boucle d'affrontement et la difficulte Souls-like.
* Livrables :
  * Premier type d'ennemi dote d'une State Machine (Patrouille, Poursuite, Attaque telegraphee, Mort).
  * Systeme de points de vie unifie (joueur et ennemi).
  * Systeme de parade et de contre-attaque.
  * Boucle de mort du joueur avec reapparition a un point de controle (Sanctuaire).

---

## Jalon 3 : Integration du Lore & Exploration (Semaines 7 a 9)
* Objectif : Donner du sens au monde et permettre la decouverte d'objets.
* Livrables :
  * Architecture de donnees ScriptableObjects pour les objets et fragments de lore.
  * Trigger d'interaction avec le monde (steles, corps au sol a fouiller).
  * Interface d'inventaire / journal affichant les descriptions narratives d'objets.
  * Premiers elements de level design connectant 3 a 4 salles avec secrets.

---

## Jalon 4 : Direction Artistique & Ambiance (Semaines 10 a 12)
* Objectif : Remplacer les placeholders par les premiers vrais visuels Pixel Art.
* Livrables :
  * Spritesheets animes du joueur (Idle, Course, Roulade, Attaques, Mort).
  * Premier tileset complet (murs, sols, gravats, vegetation corrompue).
  * Eclairage URP 2D (lanternes, brume, particules de poussieres, ombres 2D).
  * Sound design initial (bruits de pas, coups de lame, audio d'ambiance).

---

## Jalon 5 : La Tranche Verticale (Vertical Slice)
* Objectif : Une demonstration autonome d'un niveau complet.
* Contenu :
  * 1 niveau complet (Ruines exterieures menant a une crypte).
  * 2 types de monstres normaux.
  * 1 Boss de fin de zone avec cinematique de presentation legere et 2 phases.
  * 3 fragments de lore majeurs revelant une partie du mystere central.
