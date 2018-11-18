---
description: 大まかな画面のイメージと画面ごとのやりたいことを書く
---

# 2. 画面説明

## 画面イメージ

![](../.gitbook/assets/mein.png)

## 1.共通部分

画面の外郭、フレーム構造とかの説明。

### 1.1. メニュー部分

上についてるボタンが並んでいるやつ、ファイルの入出力とかする時に使うボタンをとりあえず並べておく。

{% page-ref page="2.1.-gong-tong-bu-fen/211-meny.md" %}

### 1.2. 左サイド部分

左に出ているパネル。画面の選択に使う。

{% page-ref page="2.1.-gong-tong-bu-fen/212-saido.md" %}

### 1.3. メインコンテンツ表示部分

画面ごとにいろいろ表示する。画面ごとの説明は↓に書く。

{% page-ref page="2.1.-gong-tong-bu-fen/213-meinkontentsu.md" %}

## 2. 入力シート

ユーザがデータ入力するときに使う画面たち。

### 2.1. プロジェクト設定

プロジェクトの期間、作業工程、機能、休日設定を入力する。他にもプロジェクト全体に影響しそうな情報があればこの画面に追加する。

{% page-ref page="22-shto/221-purojikuto.md" %}

### 2.2. 要員

プロジェクトに従事するリソースを入力する。

{% page-ref page="22-shto/2.2.2.-yao.md" %}

### 2.3. WBS

タスクを入力する画面。よく使われてる表のイメージ。

{% page-ref page="22-shto/2.2.3.-wbs.md" %}

### 2.4. 工程間依存

作業工程間に依存関係がある場合にそれを入力する画面。工程間の依存関係とはこの作業工程が完了しないとこっちの工程が開始できない。とかそういうやつ。

{% page-ref page="22-shto/2.2.4.-gong-cheng-yi-cun.md" %}

### 2.5. 機能間依存

機能間に依存関係がある場合にそれを入力する画面。機能間の依存関係とは（略）

{% page-ref page="22-shto/2.2.5.-neng-yi-cun.md" %}

### 2.6. PERT

タスク間に依存関係がある場合にそれを入力する画面。他の入力画面と少し毛色が異なり、PERT上のフロートやクリティカルパスを表示する。

{% page-ref page="22-shto/2.2.6.-pert.md" %}

## 3. 帳票

ユーザが見たい情報を表示する画面その１。基本的に入力操作は行わない。主に表とかテキストベースの何かの表示を想定。

### 3.1. 全体スケジュール

工程ごとの開始予定日と終了予定日を表示する。それだけ。

{% page-ref page="2.3.-piao/231-sukejru.md" %}

### 3.2. 計画工数

日付×工程ごとにBAC\(Budget at Completion\)とPV\(Planned Value\)の一覧を表示する。PVはこの日の時点で終わってなければならない作業量。BACはプロジェクト完了時点でのPVの値を想定。EVMで調べよう。

{% page-ref page="2.3.-piao/2.3.2.-hua-gong-shu.md" %}

## 4. グラフ

ユーザが見たい情報を表示する画面その２。帳票とは異なり多少情報入力しつつ入力内容から各種グラフを生成&表示する。

### 4.1. 全体ガントチャート

WBSで入力されている情報をもとにガントチャートを表示する。それだけ。実績入力とかはしない。

{% page-ref page="24-gurafu/241-gantochto.md" %}

### 4.2. 稼働スケジュール

WBSで入力された情報をもとに一人一人の日々の作業予定量とかトータルの作業量をグラフに表示する。

{% page-ref page="24-gurafu/242-sukejru.md" %}

### 4.3. PERT図

PERTで入力された情報をもとにPERT図を表示する。エッジ=タスクの図を表示する想定。

{% page-ref page="24-gurafu/2.4.3.-pert.md" %}
