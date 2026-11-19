# 03 - Narration, Monde Vivant & Cinématiques

## 1. Philosophie Narrative : Un Monde Vivant et Incarné

Contrairement aux jeux "post-apocalyptiques" où le monde n'est qu'un cimetière de ruines silencieuses déchiffré sur des cadavres, l'univers d'**Haxolot** est **profondément vivant, animé et réactif** :
* **Des civilisations et des peuples actifs** : Des villages peuplés, des campements de mercenaires, des marchés bruyants et des bastions de factions en plein essor.
* **Des PNJ avec une voix et une personnalité** : Les personnages ne sont pas de simples donneurs de quêtes cryptiques. Ils ont des motivations propres, des relations, des conflits d'intérêts et réagissent aux actions du joueur.
* **Une dramaturgie mise en scène** : L'intrigue avance par le biais de **dialogues scénarisés**, d'échanges rythmés et de **cinématiques en jeu** soulignant la tension des rencontres majeures.

---

## 2. Piliers Narratifs & Univers

### A. Le Conflit Central du Monde
* L'équilibre du continent est menacé non pas par un passé déjà mort, mais par une crise **actuelle et imminente** (ex: guerre de ressources mystiques, soulèvement d'une entité ancestrale scellée, rupture d'un pacte entre factions).
* Le monde subit les répercussions visibles de ce conflit : réfugiés, routes commerciales coupées, avant-postes assiégés et faune déstabilisée.

### B. Les Grandes Factions en Présence
1. **La Coalition Citadine / L'Ordre Régnant** : Tente de maintenir le contrôle politique et la stabilité par la loi et l'acier, quitte à réprimer les dissidences.
2. **Les Érudits de la Faille / L'Alliance Mystique** : Cherchent à maîtriser la source d'énergie primordiale du monde pour faire progresser la société, bravant les tabous.
3. **Les Tribus Sauvages / Marginaux** : Vivent en harmonie avec les forces brutes de la nature et rejettent la technologie et les dogmes des cités.

### C. Le Protagoniste et ses Relations
* Le protagoniste n'est pas un spectateur passif : il possède une identité, un ancrage dans le monde et noue des alliances au fil de sa progression.
* Des compagnons de route ou rivaux réguliers croisent son chemin, offrant un soutien moral, des dialogues piquants et des confrontations mémorables.

---

## 3. Les Systèmes Narratifs Techniques

### A. Le Système de Dialogues Scénarisés
* **Interface Dialogue (DialogueBox UI)** :
  * Affichage des textes façon RPG moderne (effet machine à écrire personnalisable).
  * Portraits expressifs des personnages en Pixel Art (visages animés, émotions : colère, doute, sourire).
  * Choix de répliques permettant d'influencer la tournure des conversations ou de débloquer des quêtes secondaires.
* **Architecture de Données** :
  * Utilisation de fichiers ScriptableObjects ou d'un outil nodal pour séquencer les arbres de dialogues de manière fluide.

### B. Mises en Scène & Cinématiques en Jeu (In-Engine Cutscenes)
* **Transitions dynamiques vers les Boss** :
  * Le joueur arrive dans la zone en vue Top-Down.
  * La cinématique démarre : la caméra (Cinemachine) glisse, recadre la scène en vue latérale, le boss s'avance avec une réplique marquante et dégaine son arme.
  * La cinématique se fond instantanément dans le combat sans temps de chargement ni écran noir.
* **Cinématiques de fin de combat** :
  * À la mort d'un boss majeur, un dialogue final ou une cinématique d'adieu scelle le destin du personnage avant le retour à l'exploration libre.
