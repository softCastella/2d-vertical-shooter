# 🚀 Space Shooter — 2D 종스크롤 슈팅 게임

Unity 2D로 제작한 종스크롤(Vertical Scroll) 슈팅 게임입니다.

---

## 📋 프로젝트 개요

| 항목 | 내용 |
|------|------|
| 엔진 | Unity (2D, URP) |
| 언어 | C# |
| 장르 | 종스크롤 슈팅 (Vertical Shooter) |
| 씬 | `GameScene` |

---

## 🎮 조작 방법

| 키 | 동작 |
|----|------|
| `방향키` / `WASD` | 플레이어 이동 |
| `Space` | 총알 발사 |
| `A` | 파워 단계 전환 (1 → 2 → 3 → 1 순환) |

---

## ⚙️ 게임 시스템

### 플레이어 (`PlayerCont.cs`)

- **HP 100** — 적 총알에 맞으면 데미지만큼 HP 감소
- **라이프 3개** — HP가 0이 되면 라이프 1개 소모, 라이프가 모두 소진되면 게임 오버
- **화면 경계 제한** — 카메라 뷰포트 기준으로 화면 밖으로 나가지 못하도록 이동 범위 제한
- **애니메이션 연동** — 좌/우 이동 시 스프라이트 애니메이션 자동 전환 (Animator 파라미터 `state` 사용)

#### 파워 단계별 발사 패턴

| 파워 | 패턴 |
|------|------|
| 1 | 중앙 1발 |
| 2 | 좌우 2발 |
| 3 | 중앙 1발 + 좌우 2발 (총 3발) |

---

### 적 (`Enemy.cs`)

적은 3가지 타입이 있으며, 타입별로 스탯과 행동이 다릅니다.

| 타입 | HP | 속도 | 발사 간격 | 경험치(점수) | 특징 |
|------|----|------|-----------|-------------|------|
| A | 80 | 1 | 2초 | 10 | 기본형 |
| B | 100 | 8 | 1.5초 | 15 | 가장 빠름 |
| C | 200 | 0.5 | 3초 | 20 | 가장 느리고 체력이 높음, 쌍발 조준 사격 |

#### 적 행동

- **이동** — GameManager가 설정한 `moveDirection`(월드 방향)으로 매 프레임 이동
- **피격 시** — 0.1초간 피격 스프라이트로 변경 후 원래 스프라이트로 복구
- **타입 C 발사** — 2개의 총구(FirePoint)에서 플레이어 위치를 조준해 동시에 발사
- **충돌 처리**
  - 플레이어 총알 → 데미지 처리 후 총알 제거
  - 플레이어 본체 → 플레이어·적 모두 제거 후 게임 오버

---

### 적 스폰 (`GameManager.cs`)

- **1~3초** 간격으로 랜덤하게 적 생성
- 스포너 종류
  - `EnemySpawner0` (위쪽) — `spawnPoint_0~4` 중 랜덤 위치에서 아래 방향으로 낙하
  - `EnemySpawner1~4` (좌우) — `startPoint` → `endPoint` 방향으로 가로 이동

---

### 총알

#### 플레이어 총알 (`PlayerBullet.cs`)
- 위쪽(`Vector3.up`)으로 직선 이동
- 화면 밖으로 나가면 `OutOfBoundsDestroy`에 의해 자동 제거

#### 적 총알 (`EnemyBullet.cs`)
- 발사 시 설정된 방향(`moveDirection`)으로 직선 이동 (월드 좌표 기준)
- 플레이어에 닿으면 데미지를 주고 총알 제거
- 화면 밖으로 나가면 `OutOfBoundsDestroy`에 의해 자동 제거

---

### 게임 오버 & 재시작 (`GameManager.cs`)

- 라이프가 모두 소진되거나 플레이어와 적이 충돌 시 게임 오버
- 게임 오버 시 씬의 모든 적·총알 일괄 제거
- **Game Over 패널** + **Retry 버튼** 표시
- Retry 클릭 → `GameScene` 씬 리로드 (라이프·HP·점수 모두 초기화)

---

### 점수 시스템

- 적 처치 시 해당 적의 `exp` 값만큼 점수 누적
- TextMeshPro UI(`scoreText`)에 실시간 반영

---

## 📁 스크립트 구조

```
Assets/
├── PlayerCont.cs        # 플레이어 이동, 발사, HP, 충돌 처리
├── PlayerBullet.cs      # 플레이어 총알 이동
├── Enemy.cs             # 적 이동, 발사, 피격, 충돌 처리
├── EnemyBullet.cs       # 적 총알 이동 및 플레이어 충돌 처리
├── GameManager.cs       # 적 스폰, 점수, 라이프, 게임 오버 관리
└── OutOfBoundsDestroy.cs# 화면 밖 오브젝트 자동 제거 (범용)
```

---

## 🏷️ Unity Tag 설정 (필수)

| 오브젝트 | Tag |
|----------|-----|
| 플레이어 | `Player` |
| 적 프리팹 | `Enemy` |
| 플레이어 총알 프리팹 | `PlayerBullet` |
| 적 총알 프리팹 | `EnemyBullet` |

---

## 🔧 Inspector 연결 항목

### GameManager 오브젝트
- `enemies` — 적 프리팹 배열 (Enemy A, B, C)
- `enemySpawners` — 스포너 오브젝트 배열
- `life_0` / `life_1` / `life_2` — 라이프 UI 오브젝트
- `scoreText` — TextMeshPro 점수 텍스트

### PlayerCont 오브젝트
- `firePoint` — 총구 Transform
- `playerBulletPrefab0` — 파워3 가운데 총알 프리팹
- `playerBulletPrefab1` — 기본 총알 프리팹

### Enemy 프리팹
- `enemyBulletPrefab1` — 적 총알 프리팹 (타입 C 전용 쌍발)
- `sprites` — [0] 기본 스프라이트, [1] 피격 스프라이트
- 자식 오브젝트에 이름에 `FirePoint` 포함된 오브젝트 필요 (타입 C: 2개)
