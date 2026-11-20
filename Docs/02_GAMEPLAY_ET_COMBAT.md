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

## 3. Le Cycle des 7 Voies en Combat : Switch Dynamique & Cooldowns

Le système de magie ne repose pas sur des menus lents, mais sur un **cycle d'actions martiales rapides** que le joueur fait tourner manette en main.

```text
┌────────────────────────────────────────────────────────────────────────┐
│                    LE CYCLE D'ACTION DES 7 VOIES                       │
├────────────────────────────────────────────────────────────────────────┤
│ 1. ASPIRER   : Capter l'énergie (drain d'endurance, recharge d'éther). │
│ 2. ALTÉRER   : Changer la nature de la lame (déchirure, bris du ciel). │
│ 3. CONDENSER : Coup lourd d'une densité colossale (allumage stellaire).│
│ 4. ACCÉLÉRER : Déviation parfaite et vélocité sans friction (Sekiro). │
│ 5. PROJETER  : Lames d'air et ondes de choc tranchantes à distance.   │
│ 6. GUIDER    : Piloter et orienter l'énergie (L'art du Sixième qui     │
│                synchronise le Double et guide le flux temporel).       │
│ 7. SCELLER   : La 7ème Voie ultime (Le Sceau, verrouillage absolu).   │
└────────────────────────────────────────────────────────────────────────┘
```

### A. Progression du Novice au Maître (Évolution des Actions de Base)
* **Le Début de l'Aventure (L'Étranger vulnérable)** :
  * Le héros arrive avec un équipement de fortune : son blocage est un coup de bouclier lourd à récupération lente, son esquive est une simple roulade au sol.
* **L'Imprégnation des Voies (L'Ascension martiale)** :
  * Au fil des découvertes et des blasphèmes, les actions de base mutent :
    * La parade devient la **Déviation Parfaite** (Voie 4 : Accélérer).
    * L'attaque chargée s'imprègne de **densité solaire écrasante** (Voie 3 : Condenser).
    * Le coup prend une **onde déchirante** capable de fendre l'espace (Voie 2 : Altérer).

### B. Schéma de Contrôles à la Manette (Zéro Menu en Combat)

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

### C. La Règle des Cooldowns Croisés & Surchauffe
Pour éviter que le joueur ne campe sur une seule Voie :
1. **Dépense & Cooldown de Voie** :
   * Lancer une compétence puissante d'une Voie (ex: *Condenser*) vide sa jauge d'énergie et la place en court cooldown.
2. **Le Bonus de Switch (L'Incitation active)** :
   * Switcher immédiatement sur une autre Voie (ex: *Accélérer*) permet de combattre sans temps mort.
   * **Mécanique de Recharge Accélérée** : Frapper ou dévier avec la Voie active accélère drastiquement le rechargement de la Voie précédente en cooldown !
   * Le joueur est ainsi naturellement poussé à faire tourner son cycle de combat.

### D. La 6ème Voie (Guider) & L'Apparition du Double
* La 6ème Voie (Guider) est débloquée lorsque le héros apprend à orienter l'énergie avec son propre esprit.
* **En Combat** :
  * Quand le joueur réussit à enchaîner plusieurs Voies du cycle en rythme, la 6ème Voie prend le relais : **le Double des Limbes jaillit automatiquement** pour guider et synchroniser une frappe en duo ou dévier une attaque fatale.
* **Au Climax** :
  * C'est cette Voie (Guider) qui permet au héros de diriger l'énergie vers le passé à travers les Limbes pour transmettre le Grimoire complet au Septième.

### E. L'Ultime Étape : La 7ème Voie (Le Sceau)
* Débloquée au climax une fois les 6 premières Voies portées à maturité dans le Grimoire du Sixième.
* Permet d'activer des arts d'interdiction absolue : geler l'arène, briser la magie des faux dieux réincarnés, et poser le sceau définitif.
