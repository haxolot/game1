# 05 - Standards Techniques et Direction Artistique

## 1. Architecture du Code Unity (C#)

### Principes
* **Modularité & Composition** : Privilégier des composants spécialisés plutôt qu'une classe monolithique. Le joueur ne possède pas un script géant de 2000 lignes mais plusieurs composants autonomes.
* **Données séparées de la logique** : Utiliser les `ScriptableObject` pour configurer les stats d'armes, ennemis, dégâts et textes de lore sans modifier de code.
* **Événements découplants** : Utiliser les `Action` C# / `UnityEvent` pour déclencher le Hit Stop, la Camera Shake et les sons sans créer de dépendances directes entre les systèmes.

### Conventions de Nommage
* **Dossiers et Assets** : PascalCase (`_Project/Scripts/Characters/Player/PlayerController.cs`).
* **Classes et Méthodes** : PascalCase (`TakeDamage()`, `StartDodge()`).
* **Variables privées** : camelCase avec underscore (`private float _currentStamina;`).
* **Variables publiques ou sérialisées** : camelCase (`[SerializeField] private float moveSpeed;`).

---

## 2. Architecture Hybride Joueur (Top-Down & Side-Scroller)

Pour gérer la double perspective (**Exploration Top-Down 3/4** et **Boss en vue latérale / Fighter Sekiro 2D**), on sépare strictement les responsabilités :

### Organisation Épurée des Scripts (`_Project/Scripts/Characters/Player/`)
On évite la sur-fragmentation en micro-scripts (pas de `PlayerStamina` ou `PlayerDodge` isolés). On ne garde que les **gros composants majeurs** :

```text
Scripts/Characters/Player/
├── PlayerController.cs            <-- Chef d'orchestre : Stats vitales (Vie, Stamina), événements et coordination
├── PlayerMovementTopDown.cs       <-- Tout le mouvement d'exploration : Marche 8 directions + Dash intégré avec i-frames
├── PlayerMovementSideScroll.cs    <-- (Sprint 4) Mouvement latéral Boss : Gravité, course, saut, contre-saut
└── PlayerCombat.cs                <-- (Sprint 2) Combat pur : Combo 3 coups, parade Sekiro, posture
```

### Mécanisme de Bascule (Switch de Perspective)
* **En Exploration** : `PlayerMovementTopDown` est activé, `rb.gravityScale = 0`.
* **En Combat de Boss** : Un événement ou déclencheur d'arène bascule le joueur :
  * `PlayerMovementTopDown.enabled = false`
  * `PlayerMovementSideScroll.enabled = true`
  * `rb.gravityScale = 3f` (activation de la physique de saut et retombée)
* **Bénéfice majeur** : Aucun conflit physique, code ultra lisible et évolutif. Le mode Top-Down est codé dès le Sprint 1, le mode Side-Scroll s'ajoute au Sprint 3 sans toucher au code existant.

---

## 3. Architecture des Scènes (Additive Scene Loading)

Pour travailler efficacement à 3 (**Haitem, Noa, Ousmane**) sans conflits Git/UVC :

### Les 3 types de scènes
1. **Scène Persistante (`Core.unity` dans `_Project/Scenes/Core/`)** :
   * Reste chargée en continu (`DontDestroyOnLoad`).
   * Contient : `GameManager`, `AudioManager`, le Canvas UI global (`HUD`), et le `Player`.
2. **Scènes d'Environnement (Chargées en Additif `LoadSceneMode.Additive`)** :
   * Ne contiennent QUE le décor, les colliders de pièces, les pièges et les spawns d'ennemis.
   * Exemples : `Overworld_Zone01.unity`, `Dungeon_Room02.unity`, `BossArena_01.unity`.
   * Permet de changer de salle sans coupure audio et sans réinitialiser les PV/stamina du joueur.
3. **Scène de Développement (`Playground.unity` dans `_Project/Scenes/Playground/`)** :
   * Laboratoire de test autonome où l'équipe calibre la physique, les contrôles et les combats sur des formes simples (Greyboxing).

---

## 4. Workflow des Prefabs

* **Règle d'or** : Tout élément de jeu important (Joueur, Ennemis, Projectiles, HUD) doit exister sous forme de **Prefab** dans `_Project/Prefabs/`.
* Le GameObject est d'abord monté dans la hiérarchie de la scène, puis glissé dans le dossier correspondant :
  * `_Project/Prefabs/Characters/Player.prefab`
  * `_Project/Prefabs/Combat/`
  * `_Project/Prefabs/UI/`
* Permet de réutiliser le joueur dans n'importe quelle scène et de propager les réglages instantanément.

---

## 5. Configuration Rendu & Pixel Art (URP 2D)

### Paramètres d'Importation des Sprites
* **Texture Type** : Sprite (2D and UI)
* **Sprite Mode** : Single ou Multiple
* **Pixels Per Unit (PPU)** : 16 (ou 32 selon la résolution retenue, à standardiser).
* **Filter Mode** : Point (no filter) - indispensable pour préserver la netteté des pixels.
* **Compression** : None (évite les artefacts sur les contours).

### Sorting Layers Recommandées (Ordre d'affichage)
1. `Background_Far` (Ciel, décors lointains)
2. `Background_Near` (Murs du fond, structures arrières)
3. `Terrain` (Sol, obstacles, collisions)
4. `Props_Back` (Éléments de décor derrière les entités)
5. `Entities` (Joueur, ennemis, boss, projectiles)
6. `Props_Front` (Piliers avant, gravats masquant le premier plan)
7. `VFX` (Étincelles, flashs, sang)
8. `Lighting` (Lumière 2D globale et spots)
9. `UI` (Barres de vie, menus, textes)

---

## 6. Pipeline Aseprite
* Le package Unity Aseprite est intégré au projet.
* Tous les fichiers source `.aseprite` peuvent être déposés directement dans `_Project/Art/Sprites/` : Unity génère automatiquement les slices et les animations.
