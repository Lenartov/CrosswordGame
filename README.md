# CrosswordGame

ОПИС:
Гравець з’єднує літери в колі, формуючи слова, які заповнюють клітинки кросворду. Щоб перемогти, потрібно заповнити весь кросворд правильними словами.

СКРІНШОТИ:
![image](https://github.com/user-attachments/assets/34e5d4a3-d9ce-422c-a4ce-7d9c212210d2)
![image](https://github.com/user-attachments/assets/a70d05ad-458a-41a8-8039-b492f27d6093)
![image](https://github.com/user-attachments/assets/5d86884c-5c32-41de-87e5-dcf4ad73101d)

ТЕХНІЧНИЙ ОПИС:
Дані про рівнів (слова та букви) зберігаються в окремих JSON файлах, в папці GameData. 
Кросфорд генерується автоматично, на основі масива слів.
Для додавання нового рівня: потрібно новий файл додати до ігрового об'єкта LevelDataManager, що на сцені LevelSelectionMenu.

