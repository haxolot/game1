# 04 - Roadmap et Jalons de Production

## Vue d'Ensemble des Jalons (Milestones)

L'objectif de l'équipe (**Haitem, Noa, Ousmane**) est la réalisation d'une **Vertical Slice** (tranche verticale jouable de 10 à 15 minutes) démontrant la force de la formule hybride : **Exploration vivante en Top-Down + Duel de Boss électrisant en Side-Scroller (Sekiro 2D)**.

---

## Jalon 1 : Mouvement, Inputs & Playground (Sprint 1 - EN COURS)
* **Objectif** : Valider les sensations physiques du joueur dans une arène sans graphismes finaux (Greybox).
* **Livrables** :
  * [x] Architecture du projet et dépôt UVC (`_Project/`).
  * [x] Contrôleur de déplacement 8 directions Top-Down (`PlayerMovementTopDown.cs`).
  * [x] Jauge d'endurance réactive (`PlayerStamina.cs`).
  * [x] Dash vif avec invulnérabilité (*i-frames*) (`PlayerDodge.cs`).
  * [x] Prise en charge New Input System clavier/manette (`InputReader.cs`).
  * [x] Prefab du Joueur configuré (`Player.prefab`).

---

## Jalon 2 : Cœur du Combat "Sekiro 2D" & Game Feel (Sprint 2)
* **Objectif** : Implémenter le système de parade parfaite, de posture et les retours sensoriels viscéraux.
* **Livrables** :
  * Système de Hitbox/Hurtbox universel (`IDamageable.cs`).
  * Combo de base à l'épée en 3 coups.
  * Système de Déviation / Parade stricte (*Sekiro-like*) avec sons métalliques (*CLANG*).
  * Jauge de Posture pour le joueur et les cibles.
  * Moteur de Game Feel : Hit Stop (micro-gel du temps), secousses de caméra directionnelles (*CameraShake*) et flashs blancs d'impact.
  * Mannequin d'entraînement (*Training Dummy*) pour tester immédiatement le duel.

---

## Jalon 3 : Système de Dialogues & IA Ennemis (Sprint 3)
* **Objectif** : Animer le monde vivant et créer le premier monstre d'exploration.
* **Livrables** :
  * Système de boîte de dialogue avec portraits animés et choix de texte.
  * Premier PNJ interactif avec répliques scénarisées.
  * IA d'ennemi régulier en vue Top-Down (Patrouille, Détection, Assaut télégraphié).
  * Système de points de vie unifié et boucle de mort/réapparition.

---

## Jalon 4 : Le Boss en Duel Latéral & Transition Cinématique (Sprint 4)
* **Objectif** : Réaliser la bascule hybride Top-Down $\rightarrow$ Side-Scroller.
* **Livrables** :
  * Contrôleur Joueur en vue latérale (`PlayerMovementSideScroll.cs` : gravité, saut, estocade, contre-saut).
  * Déclencheur cinématique d'arène : transition de caméra fluide Top-Down vers vue latérale.
  * IA du premier Boss en vue de profil avec attaques périlleuses (balayage bas et estocade).
  * Écran de Coup de Grâce (*Deathblow*) lors du bris de posture du boss.

---

## Jalon 5 : Habillage Graphique, Éclairage URP 2D & Audio (Sprint 5)
* **Objectif** : Passer du prototype Greybox à l'ambiance Pixel Art vivante.
* **Livrables** :
  * Intégration des spritesheets du joueur et du boss.
  * Décors en Pixel Art et éclairage dynamique URP 2D (lanternes, flammes, ombres).
  * Sound design complet (impacts, ambiance de cité, musique de duel épique).

---

## Jalon 6 : La Vertical Slice Finale (Sprint 6)
* **Livrables** :
  * 1 village/campement vivant avec dialogues et quête d'introduction.
  * 1 zone d'exploration avec ennemis de faune locale.
  * 1 cinématique d'introduction de boss débouchant sur un combat dantesque en duel de profil.
  * Équilibrage complet manette en main.
