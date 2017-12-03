import random

MAX_CYCLE = 10000
ALPHA = 0.1
EPSILON = 0.3
GAMMA = 0.99
q = [[[[[ 0 for i in range(4)] for j in range(5)] for k in range(100)] for l in range(100)] for m in range(14)]

def getLimit(coin):	#coinを見てどこまで買えるかを返す
	if coin < 3:
		return 0
	elif coin < 6:
		return 1
	elif coin < 8:
		return 2
	else:
		return 3

def getAction(coin, silver, gold, win_point):
	limit = getLimit(coin)
	
	if random.random() < EPSILON:	#ランダムな手を指す
			return random.randint(0,limit)
			
	else:	#Q値が一番良い行動を取る
		max_q = max(q[coin-2][silver][gold][win_point][0:limit+1])
		index = q[coin-2][silver][gold][win_point][0:limit+1].index(max_q)
		return index
		
	return -1 #エラーを投げる
	
def getGreedAction(coin, silver, gold, win_point):	#Q値が一番良い行動を取る(方策評価用のメソッド)
	limit = getLimit(coin)
	max_q = max(q[coin-2][silver][gold][win_point][0:limit+1])
	index = q[coin-2][silver][gold][win_point][0:limit+1].index(max_q)
	return index
		
	return -1  #エラーを投げる

def updateQ(coin, silver, gold, win_point, act, reward):
	if act == 1:
		silver += 1
	elif act == 2:
		gold += 1
	elif act == 3:
		win_point += 1
	limit = getLimit(coin)
	max_q = max(q[coin-2][silver][gold][win_point][0:limit+1])
	q[coin-2][silver][gold][win_point][act] = (1 - ALPHA) * q[coin-2][silver][gold][win_point][act] + ALPHA * (reward + GAMMA * max_q)


def main():
	for cycle in range(0,MAX_CYCLE):
		deck = [0, 0, 0, 1, 1, 1, 1, 1, 1, 1]
		turn = 0
		silver = 0
		gold = 0
		win_point = 0
		act = 0
		coin = 0
		reward = 0
		
		if(cycle % 100 != 0):
			while win_point < 4:
				turn += 1
				random.shuffle(deck)
				tefuda = deck[0:5].copy()
				coin = sum(tefuda)
				act = getAction(coin, silver, gold, win_point)
				if win_point == 4 and act == 3:
					reward = 100.0
				else:
					reward = -1.0
				updateQ(coin, silver, gold, win_point, act, reward)
				if act == 1:
					silver += 1
					deck.append(2)
				elif act == 2:
					gold += 1
					deck.append(3)
				elif act == 3:
					win_point += 1
					deck.append(0)
		else:
			while win_point < 4:
				turn += 1
				random.shuffle(deck)
				tefuda = deck[0:5].copy()
				coin = sum(tefuda)
				act = getGreedAction(coin, silver, gold, win_point)
				if win_point == 4 and act == 3:
					reward = 100.0
				else:
					reward = -1.0
#				updateQ(coin, silver, gold, win_point, act, reward) 方策の評価用なので学習はしない
				if act == 1:
					silver += 1
					deck.append(2)
				elif act == 2:
					gold += 1
					deck.append(3)
				elif act == 3:
					win_point += 1
					deck.append(0)
				if 999 < turn:	#グリーディーはハマると抜けられないのでブレークしとこ
					break
			print(str(cycle+1) + "回目:" + str(turn) + "ターン")
			print(deck)
			print("-------------------")