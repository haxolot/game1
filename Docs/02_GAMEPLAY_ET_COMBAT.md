# 02 - Gameplay et Systèmes de Combat

## 1. La Double Boucle de Gameplay (Formule Hybride)

Le jeu sépare clairement le rythme de l'aventure en deux modes complémentaires :

```text
┌────────────────────────────────────────────────────────┐
│ MODE A : EXPLORATION & FAUNE (TOP-DOWN 3/4)            │
│ • Déplacement multidirectionnel fluide (gravité = 0). │
│ • Découverte de régions vivantes, cités, donjons.      │
│ • Dialogues scénarisés avec les PNJ et quêtes.         │
│ • Escarmouches dynamiques contre la faune locale.      │
└──────────────────────────┬─────────────────────────────┘
                           │ TRANSITION CINÉMATIQUE (Entrée de Boss)
┌──────────────────────────▼─────────────────────────────┐
│ MODE B : DUELS DE BOSS & SITUATIONS CLÉS (SIDE-SCROLL) │
│ • Caméra latérale verrouillée façon jeu de combat.     │
│ • Gravité active, mécanique de saut et de sol.         │
│ • Système Sekiro 2D : Déviations, Posture, Clangs.    │
│ • Coups de grâce spectaculaires (Deathblows).          │
└────────────────────────────────────────────────────────┘
```

---

## 2. Le Cœur du Combat : Système "Sekiro 2D" (Mode Boss / Vue Latérale)

C'est la priorité technique et de game design absolue du projet (inspiré de ***Nine Sols*** et ***Sekiro***).

### A. Jauge de Posture / Équilibre
* Le Boss et le Joueur possèdent tous deux une **jauge de Posture** (en plus de leurs PV).
* Bloquer une attaque adverse sans timing parfait absorbe les dégâts mais remplit la posture du joueur.
* **La Déviation (Parade Parfaite)** :
  * Timing strict (fenêtre active de 0.12s à 0.15s).
  * Annule tout dégât pour le joueur et inflige d'énormes dégâts de posture à l'adversaire.
* **Le Coup de Grâce (Deathblow)** :
  * Lorsque la jauge de posture du boss est saturée, il entre en état d'étourdissement critique (*Posture Break*).
  * Une invite visuelle permet au joueur de déclencher une exécution cinématique lourde qui retire une barre de vie entière au boss.

### B. Lecture de Patterns & Attaques Périlleuses
Le boss possède des attaques spéciales impossibles à bloquer simplement, annoncées par une alerte visuelle et sonore :
1. **Le Balayage Bas** :
   * Impossible à parer au sol.
   * Contre requis : **Sauter par-dessus** l'arme et rebondir sur le boss pour lui infliger un choc de posture.
2. **L'Estocade Perçante** :
   * Transperce la garde normale.
   * Contre requis : **Contre au Dash avant (Mikiri 2D)**, où le joueur dash dans la pointe pour stopper net le coup et punir l'ennemi.

### C. Game Feel Viscéral (Priorité Feedback)
* **Le CLANG Métallique** : Un bruit de choc de lames strident et sec qui valide auditivement chaque déviation réussie.
* **Hit Stop** : Micro-gel de l'image (0.04s à 0.08s) à chaque impact de sabre ou parade pour donner du poids et du tranchant.
* **Camera Shake & Flash** : Secousses d'écran directionnelles proportionnelles à la violence du coup et clignotement blanc des silhouettes touchées.
* **Input Buffering & Animation Cancelling** : Le joueur peut annuler la fin d'un coup de base pour lever sa parade immédiatement si le boss riposte, assurant une réactivité totale.

---

## 3. Le Cycle des Voies en Combat : Switch Dynamique & Cooldowns

Le système de combat ne repose pas sur des menus lents, mais sur un **cycle d'actions martiales rapides** que le joueur fait tourner manette en main. Le combat reste **strictement centré sur le sabre et la virtuosité solo du héros** (le Double des Limbes est une ancre métaphysique narrative, aucune invocation ou clone en combat).

```text
┌────────────────────────────────────────────────────────────────────────┐
│                    LE CYCLE D'ACTION DES VOIES DU HÉROS                │
├────────────────────────────────────────────────────────────────────────┤
│ 1. ASPIRER   : Capter l'énergie (drain d'endurance, recharge d'éther). │
│ 2. ALTÉRER   : Changer la nature de la lame (déchirure, bris du ciel). │
│ 3. CONDENSER : Coup lourd d'une densité colossale (allumage stellaire).│
│ 4. ACCÉLÉRER : Déviation parfaite et vélocité sans friction (Sekiro). │
│ 5. PROJETER  : Lames d'air et ondes de choc tranchantes à distance.   │
│ 6. GUIDER    : Canaliser l'énergie pure sans perte (précision absolue) │
│                et transmission du journal vers le passé au Climax.     │
│ 7. SCELLER   : La Voie ultime (Le Sceau), verrouillage métaphysique.  │
└────────────────────────────────────────────────────────────────────────┘
```

### A. Schéma de Contrôles à la Manette (Zéro Menu en Combat)

```text
┌────────────────────────────────────────────────────────┐
│ COMMANDES MARTIALES PERMANENTES                        │
│ • X (Carré)       : Coup rapide au sabre (enchaînements)│
│ • Y (Triangle)    : Attaque lourde / bris de garde     │
│ • LT / L2         : Parade / Déviation (Timing Sekiro) │
│ • RT / R2         : Dash d'esquive / Contre avant      │
├────────────────────────────────────────────────────────┤
│ LE SYSTÈME DE VOIES DYNAMIQUE                          │
│ • LB / L1         : SWITCH DE VOIE INSTANTANÉ (Cycle)  │
│ • RB / R1         : Compétence / Sort de la Voie active│
│ • D-Pad (Croix)   : Raccourcis directs de Voies        │
└────────────────────────────────────────────────────────┘
```

### B. La Règle des Cooldowns Croisés & Surchauffe
1. **Dépense & Cooldown de Voie** :
   * Lancer une compétence puissante d'une Voie (ex: *Condenser*) vide sa jauge d'énergie et la place en court cooldown.
2. **Le Bonus de Switch (L'Incitation active)** :
   * Switcher immédiatement sur une autre Voie (ex: *Accélérer*) permet de combattre sans temps mort.
   * **Recharge Accélérée** : Frapper ou dévier avec la Voie active accélère drastiquement le rechargement de la Voie précédente en cooldown !
   * Le joueur est ainsi naturellement poussé à faire tourner son cycle de combat.

---

## 4. Boss Design Spécial : Le Duel du Rival & La Voie « ROMPRE »

Dans la **Run Égoïste**, le boss final est le Rival du héros, armé de sa Voie unique et anormale : **ROMPRE**.

### A. La Mécanique du « ONE TRY » (Pas de Game Over)
* Ce combat ne se recharge pas à la mort du joueur : **c'est un ONE TRY narratif absolu**.
* Équilibré pour être « plus stylé qu'impossible » (une danse martiale de 5 à 7 minutes au sabre, sans one-shot punitif, avec des dialogues organiques au fil des déviations).
* **Issue A (Défaite du Joueur)** : Déclenche directement la cinématique où le Rival abat le héros en larmes, prend le Sceau et sauve le monde (Fin Tragique du Rival).
* **Issue B (Victoire du Joueur)** : Déclenche la cinématique où le héros terrasse son ami, refuse le Sceau et repart avec le remède pour sa mère dans un monde condamné (Fin Sombre du Héros).

### B. Le Mind-Game de « ROMPRE » (Annulations Actives à Charges)
Pour éviter la frustration d'une immunité passive, les sorts du joueur fonctionnent normalement, mais le Rival gère une **jauge de contre actif** :
1. **Les Salves d'Annulation** :
   * Quand sa jauge de Rupture est pleine, le Rival peut annuler les sorts du joueur en plein vol d'un coup de garde sec (*CLANG de Rupture*).
   * Il peut enchaîner 1, 2 ou 3 annulations d'affilée en mode furie, forçant le joueur à rengainer sa magie pour passer en 100% parade au sabre.
2. **La Fenêtre de Vulnérabilité (Le Bait)** :
   * Après avoir dépensé ses charges de rupture, le Rival entre en cooldown de réarmement.
   * C'est le moment pour le joueur de le feinter, de forcer une annulation dans le vide, puis de lui asséner ses grosses Voies chargées (*Condenser*, *Altérer*) pendant sa récupération.
