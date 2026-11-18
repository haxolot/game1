# 02 - Gameplay et Systemes de Combat

## 1. La Boucle de Gameplay Principale (Core Loop)
1. Explorer une zone hostile, reperer des embranchements et des secrets de lore.
2. Affronter des ennemis aux comportements distincts en gerant son endurance et son positionnement.
3. Decouvrir des fragments de lore, amasser de la ressource vitale / d'amelioration.
4. Trouver un point de repos / sanctuaire pour securiser les acquis et regenerer ses ressources.
5. Vaincre le gardien de la zone (Boss) pour ouvrir l'acces aux regions suivantes.

---

## 2. Le Systeme de Combat

### A. Actions du Joueur
* Attaque de base : Combo en 3 coups. Chaque coup a une portee, des degats et un cout d'endurance croissants.
* Esquive / Roulade : Mouvement rapide accordant des frames d'invulnerabilite (i-frames). Cooldown court, couteuse en endurance.
* Parade (Parry) : Fenetre de timing courte juste avant l'impact adverse. Une parade reussie etourdit l'adversaire et autorise un coup critique.
* Soin / Relique de restauration : Utilisation limitee d'une ressource de soin, avec une animation qui immobilise brievement le joueur (necessite de trouver le bon timing).

### B. Gestion de l'Endurance (Stamina)
* Barre d'endurance visible sous les PV.
* Actions consommatrices : Attaque, Dash/Roulade, Blocage.
* Regles :
  * Si la barre tombe a zero, le joueur entre en etat de recuperation (vitesse reduite, pas d'action pendant 1 seconde).
  * La regeneration commence apres un delai fixe (0.4s sans action consommatrice).

### C. Game Feel et Retours Sensoriels
* Hit Stop : Gel du temps du jeu pendant 0.04s a 0.08s a chaque coup porte ou recu pour donner du poids a l'impact.
* Flash blanc (Hit Flash) : Clignotement de la silhouette du sprite touche.
* Particules d'impact : Eclats directionnels selon l'angle de coupe de l'arme.
* Secousses de camera (Screen Shake) : Intensite proportionnelle a la violence du coup.

---

## 3. Conception des Ennemis

### Principes Fondamentaux
1. Telegraphe obligatoire : Toute attaque dangereuse doit avoir une phase d'anticipation reconnaissable (lueur, bruitage, posture).
2. Justesse : Les zones de collision (hitboxes) doivent matcher strictement le visuel de l'arme ou de l'impact.
3. Punition equitable : Le joueur ne doit jamais mourir sans comprendre son erreur.

### Categories d'Ennemis
* Fretin / Sentinelles : Faible vie, attaquent en groupe, apprennent les bases de l'esquive et du crowd control.
* Chevaliers / Gardiens : Armures lourdes, boucliers, attaques lourdes. Requièrent de la parade ou du contournement.
* Boss & Mini-Boss : Combats en plusieurs phases, attaques de zone (AoE), musique dediee, arènes scellees.
