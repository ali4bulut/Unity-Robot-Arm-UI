import numpy as np
import math


class RobotKol:

    def __init__(self, DH_Parametresi, UzuvUzunluklari):
        self.dh = DH_Parametresi
        self.Uzunluk = np.array(UzuvUzunluklari)
        self.EklemSayisi = len(self.dh)
        self.TransferFonksiyonlari = []

    def FK(self):
        for i in range(self.EklemSayisi):
            T = np.array([[np.cos(self.dh[i][3]), -np.sin(self.dh[i][3]), 0, self.dh[i][1]],
                          [np.sin(self.dh[i][3]) * np.cos(self.dh[i][0]), np.cos(self.dh[i][3]) * np.cos(self.dh[i][0]),
                           -np.sin(self.dh[i][0]),
                           -(self.dh[i][2] * np.sin(self.dh[i][0]))],
                          [np.sin(self.dh[i][3]) * np.sin(self.dh[i][0]), np.cos(self.dh[i][3]) * np.sin(self.dh[i][0]),
                           np.cos(self.dh[i][0]), np.cos(self.dh[i][0]) * self.dh[i][2]],
                          [0, 0, 0, 1]])
            self.TransferFonksiyonlari.append(T)
        Carpim = np.dot(self.TransferFonksiyonlari[0], self.TransferFonksiyonlari[1])
        for j in range(2, self.EklemSayisi):
            Carpim = np.dot(Carpim, self.TransferFonksiyonlari[j])
        return Carpim

    def IK(self, alfa, beta, gama, px, py, pz):
        R_zyz = self.ZYZEuler(alfa, beta, gama, px, py, pz)

        merkezKonum = self.MerkezKonumBul(R_zyz, self.Uzunluk[5], self.dh[5][1])

        a1, a2, a3, a4, a5 = self.Uzunluk[0], self.Uzunluk[1], self.Uzunluk[2], self.Uzunluk[3], self.Uzunluk[4]

        q1 = math.atan2(merkezKonum[1], merkezKonum[0])

        if 170 * (math.pi / 180) >= q1 >= -170 * (math.pi / 180):
            _px = math.sqrt(merkezKonum[0] ** 2 + merkezKonum[1] ** 2) - a2
        else:
            _px = math.sqrt(merkezKonum[0] ** 2 + merkezKonum[1] ** 2) + a2

        if merkezKonum[2] >= 0:
            _pz = abs(merkezKonum[2] - a1)
        else:
            _pz = abs(merkezKonum[2]) + a1
        try:
            k = math.sqrt((_pz ** 2 + _px ** 2))
            L = math.sqrt((a5 ** 2 + a4 ** 2))
            a = math.atan2(_pz, _px)
            c = math.atan2(a5, a4)
            b = math.acos((a3 ** 2 + L ** 2 - k ** 2) / (2 * a3 * L))
            d = math.acos((a3 ** 2 + k ** 2 - L ** 2) / (2 * a3 * k))

            if 170 * (math.pi / 180) >= q1 >= -170 * (math.pi / 180):
                q3 = c + b - math.pi
            else:
                q3 = c - b + math.pi
            if merkezKonum[2] >= a1:
                if 170 * (math.pi / 180) >= q1 >= -170 * (math.pi / 180):
                    q2 = d + a - (math.pi / 2)
                else:
                    q2 = (math.pi / 2) - a - d
                    q1 = q1 - (180 * (math.pi / 180)) if q1 > 0 else q1 + (180 * (math.pi / 180))
            else:
                if 170 * (math.pi / 180) >= q1 >= -170 * (math.pi / 180):
                    q2 = d - (math.pi / 2) - a
                else:
                    q2 = (math.pi / 2) + a - d
                    q1 = q1 - (180 * (math.pi / 180)) if q1 > 0 else q1 + (180 * (math.pi / 180))

            T0_3 = self.T03Matrisi(q1, q2, q3, a1, a2, a3)
            tT0_3 = self.TersMatrix(T0_3)
            R3_6 = np.dot(tT0_3, R_zyz)

            q5 = math.acos(-R3_6[1][2])
            q4 = math.atan2(R3_6[2][2], R3_6[0][2])
            q6 = math.atan2(-R3_6[1][1], R3_6[1][0])

            return [q1, q2, q3, q4, q5, q6]
        except:
            return [0, 0, 0, 0, 0, 0]

    @staticmethod
    def T03Matrisi(q1, q2, q3, a1, a2, a3):
        return [[-(np.cos(q1) * np.sin(q2) * np.cos(q3)) - (np.cos(q1) * np.cos(q2) * np.sin(q3)),
                 np.cos(q1) * np.sin(q2) * np.sin(q3) - (np.cos(q1) * np.cos(q2) * np.cos(q3)),
                 np.sin(q1), -(np.cos(q1) * np.sin(q2) * a3) + np.cos(q1) * a2],
                [-(np.sin(q1) * np.sin(q2) * np.cos(q3)) - (np.sin(q1) * np.cos(q2) * np.sin(q3)),
                 np.sin(q1) * np.sin(q2) * np.sin(q3) - (np.sin(q1) * np.cos(q2) * np.cos(q3)),
                 -np.cos(q1), -(np.sin(q1) * np.sin(q2) * a3) + np.sin(q1) * a2],
                [np.cos(q2) * np.cos(q3) - (np.sin(q2) * np.sin(q3)),
                 -(np.cos(q2) * np.sin(q3)) - (np.sin(q2) * np.cos(q3)), 0, np.cos(q2) * a3 + a1],
                [0, 0, 0, 1]]

    @staticmethod
    def MerkezKonumBul(Matrix, d6, d7=0):
        P_6 = [[Matrix[0][3]], [Matrix[1][3]], [Matrix[2][3]], [1]]
        a_6 = [[Matrix[0][2]], [Matrix[1][2]], [Matrix[2][2]], [0]]  # T06'nın Z açısı
        P4 = np.subtract(P_6, np.dot(d6, a_6))
        if d7 != 0:
            a_7 = [[Matrix[0][0]], [Matrix[1][0]], [Matrix[2][0]], [0]]  # T06'nın X açısı
            P4 = P4 - np.dot(d7, a_7)
        return P4

    @staticmethod
    def ZYZEuler(alfa, beta, gama, px, py, pz):
        R_zyz = [
            [np.cos(alfa) * np.cos(beta) * np.cos(gama) - (np.sin(alfa) * np.sin(gama)),
             -(np.cos(alfa) * np.cos(beta) * np.sin(gama)) - (np.sin(alfa) * np.cos(gama)), np.cos(alfa) * np.sin(beta),
             px],
            [np.sin(alfa) * np.cos(beta) * np.cos(gama) + np.cos(alfa) * np.sin(gama),
             -(np.sin(alfa) * np.cos(beta) * np.sin(gama)) + np.cos(alfa) * np.cos(gama),
             np.sin(alfa) * np.sin(beta), py],
            [-(np.sin(beta) * np.cos(gama)), np.sin(beta) * np.sin(gama), np.cos(beta), pz], [0, 0, 0, 1]]
        return R_zyz

    @staticmethod
    def TersMatrix(matrix):
        rr = [[matrix[0][0], matrix[1][0], matrix[2][0]],
              [matrix[0][1], matrix[1][1], matrix[2][1]],
              [matrix[0][2], matrix[1][2], matrix[2][2]]]
        _rr = np.dot(-1, rr)
        pp = np.dot(_rr, [[matrix[0][3]], [matrix[1][3]], [matrix[2][3]]])

        return [[rr[0][0], rr[0][1], rr[0][2], pp[0]],
                [rr[1][0], rr[1][1], rr[1][2], pp[1]],
                [rr[2][0], rr[2][1], rr[2][2], pp[2]],
                [0, 0, 0, 1]]


def main():
    a1, a2, a3, a4, a5, a6 = 328, 40.2, 445, 39.95, 438.35, 164.2
    # a1, a2, a3, a4, a5, a6 = 111, 14, 120, 14, 116, 80
    PT = [[0, 0, a1, 0],
          [np.pi / 2, a2, 0, np.pi / 2],
          [0, a3, 0, 0],
          [np.pi / 2, a4, a5, 0],
          [-np.pi / 2, 0, 0, 0],
          [np.pi / 2, 0, a6, 0]]

    uzuvlar = [a1, a2, a3, a4, a5, a6]

    robot = RobotKol(PT, uzuvlar)

    ik = robot.IK(0, np.pi / 2, np.pi, 6.42750000e+02, 0, 8.12950000e+02)

    PT2 = [[0, 0, a1, ik[0]],
           [np.pi / 2, a2, 0, ik[1] + np.pi / 2],
           [0, a3, 0, ik[2]],
           [np.pi / 2, a4, a5, ik[3]],
           [-np.pi / 2, 0, 0, ik[4]],
           [np.pi / 2, 0, a6, ik[5]]]

    robot1 = RobotKol(PT2, uzuvlar)
    sonuc = robot1.FK()
    print("-" * 50)
    print(np.matrix(sonuc))
    print("-" * 50)
    for i in range(len(ik)):
        print(f"Q{i + 1} :", end=" ")
        print(ik[i] * (180 / np.pi))


if __name__ == '__main__':
    main()
