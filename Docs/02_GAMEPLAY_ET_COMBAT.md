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

## 4. Boss Design Spécial : Le Duel Ultime contre Kaelys Prime (Le Vrai Rival)

Au Climax de l'Acte V, après le Boss Rush des 4 autres Précurseurs (Gorath, Vaelen, Lyra, Malakor), le boss final absolu est le véritable rival du héros : **Kaelys Prime**, le maître originel de la Voie de l'Altération.

### A. La Philosophie du Duel : L'Élève face au Maître
* Ce duel au sabre de 5 à 7 minutes au cœur des cyclones de l'Épicentre est le sommet de la tension *Sekiro-like*.
* Ce n'est pas un monstre géant ni une masse informe : c'est un duel d'escrimeurs de taille humaine, d'une élégance et d'une rapidité foudroyantes.
* Kaelys met à l'épreuve tout ce qu'il a enseigné, provoqué et observé chez le héros : chaque coup porté est accompagné de dialogues organiques au fil des déviations.

### B. Le Mind-Game de Kaelys Prime (Altération Suprême)
Kaelys maîtrise l'art de changer la nature du flux et le tranchant de sa lame :
1. **Les Tranchants Brise-Ciel** :
   * Des estocs et taillades d'une vélocité inouïe qui tranchent à travers la garde passive si le joueur ne réussit pas une déviation parfaite (*Perfect Parry*).
2. **Le Changement de Rythme (Feintes d'Altération)** :
   * Kaelys altère la vitesse de ses propres coups en plein élan (ralentissement visuel puis accélération brutale), forçant le joueur à observer attentivement ses bras et ses épaules pour caler le timing de parade.
3. **Le Test du Guide (Assimilation)** :
   * Le joueur doit utiliser ses passifs de **GUIDER** (Voie 6) pour dévier les lames d'éther de Kaelys et recharger sa propre posture, avant de placer les mouvements de la 7ᵉ Voie (*Sceller*) pour briser la garde divine de Kaelys.
