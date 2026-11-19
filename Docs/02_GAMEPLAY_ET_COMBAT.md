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

## 3. Le Combat d'Exploration (Mode Top-Down 3/4)

* Déplacement vif à 8 directions sans glissement.
* Combo fluide de base à l'épée en 3 coups.
* Dash multidirectionnel avec invulnérabilité (*i-frames*) pour traverser les projectiles et se repositionner.
* Gestion d'endurance (*Stamina*) assurant que le joueur alterne intelligemment entre assauts et esquives.
